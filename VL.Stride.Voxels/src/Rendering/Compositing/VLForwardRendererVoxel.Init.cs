using System.ComponentModel;
using Stride.Core;
using Stride.Core.Annotations;
using Stride.Core.Collections;
using Stride.Core.Diagnostics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Rendering.Images;
using Stride.Rendering.Lights;
using Stride.Rendering.Shadows;
using Stride.Rendering.SubsurfaceScattering;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using Stride.VirtualReality;
using VL.Stride.Rendering;
using MeshRenderFeature = Stride.Rendering.MeshRenderFeature;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    [Display("VL Forward Renderer Voxel")]
    public partial class VLForwardRendererVoxel : SceneRendererBase, ISharedRenderer
    {
        private static readonly ProfilingKey CollectCoreKey = new ProfilingKey(
            "VLForwardRendererVoxel.CollectCore"
        );
        private static readonly ProfilingKey DrawCoreKey = new ProfilingKey(
            "VLForwardRendererVoxel.DrawCore"
        );
        private static readonly ProfilingKey VoxelCollectKey = new ProfilingKey(
            "VLForwardRendererVoxel.VoxelCollect"
        );
        private static readonly ProfilingKey VoxelDrawKey = new ProfilingKey(
            "VLForwardRendererVoxel.VoxelDraw"
        );
        private static readonly ProfilingKey VoxelDebugKey = new ProfilingKey(
            "VLForwardRendererVoxel.VoxelDebug"
        );

        public const PixelFormat DepthBufferFormat = PixelFormat.D24_UNorm_S8_UInt;

        private IShadowMapRenderer shadowMapRenderer;
        private Texture depthStencilROCached;
        private MultisampleCount actualMultisampleCount = MultisampleCount.None;
        private VRDeviceSystem vrSystem;

        private readonly Logger logger = GlobalLogger.GetLogger(nameof(VLForwardRendererVoxel));
        private readonly FastList<Texture> currentRenderTargets = new FastList<Texture>();
        private readonly FastList<Texture> currentRenderTargetsNonMSAA = new FastList<Texture>();
        private Texture currentDepthStencil;
        private Texture currentDepthStencilNonMSAA;

        protected Texture viewOutputTarget;
        protected Texture viewDepthStencil;

        protected int ViewCount { get; private set; }
        protected int ViewIndex { get; private set; }

        private ViewportState currentViewportState = new ViewportState();

        public ClearRenderer Clear { get; set; } = new ClearRenderer();

        public bool LightProbes { get; set; } = false;

        public RenderStage OpaqueRenderStage { get; set; }
        public RenderStage TransparentRenderStage { get; set; }

        [MemberCollection(NotNullItems = true)]
        public List<RenderStage> ShadowMapRenderStages { get; } = new List<RenderStage>();

        public RenderStage GBufferRenderStage { get; set; }
        public IPostProcessingEffects PostEffects { get; set; }
        public LightShafts LightShafts { get; set; }
        public VRRendererSettings VRSettings { get; set; }
        public ViewportSettings ViewportSettings { get; set; } = new ViewportSettings();
        public SubsurfaceScatteringBlur SubsurfaceScatteringBlurEffect { get; set; }
        public MultisampleCount MSAALevel { get; set; } = MultisampleCount.None;

        [NotNull]
        public MSAAResolver MSAAResolver { get; } = new MSAAResolver();

        [DefaultValue(true)]
        public bool BindDepthAsResourceDuringTransparentRendering { get; set; } = true;

        [DefaultValue(false)]
        public bool BindOpaqueAsResourceDuringTransparentRendering { get; set; }

        /// <summary>
        /// The voxel renderer.
        /// </summary>
        public IVoxelRenderer VoxelRenderer { get; set; }

        /// <summary>
        /// Voxel debug visualization.
        /// </summary>
        public VoxelDebug VoxelVisualization { get; set; }

        /// <summary>
        /// Diagnostic switch: enable voxel collect.
        /// </summary>
        public bool EnableVoxelCollect { get; set; } = true;

        /// <summary>
        /// Diagnostic switch: enable voxel draw.
        /// </summary>
        public bool EnableVoxelDraw { get; set; } = true;

        /// <summary>
        /// Diagnostic switch: enable voxel debug overlay.
        /// </summary>
        public bool EnableVoxelDebugDraw { get; set; } = true;

        public VLForwardRendererVoxel() { }

        protected override void InitializeCore()
        {
            base.InitializeCore();

            shadowMapRenderer = Context
                .RenderSystem.RenderFeatures.OfType<MeshRenderFeature>()
                .FirstOrDefault()
                ?.RenderFeatures.OfType<ForwardLightingRenderFeature>()
                .FirstOrDefault()
                ?.ShadowMapRenderer;

            if (MSAALevel != MultisampleCount.None)
            {
                actualMultisampleCount = (MultisampleCount)
                    Math.Min(
                        (int)MSAALevel,
                        (int)
                            GraphicsDevice
                                .Features[PixelFormat.R16G16B16A16_Float]
                                .MultisampleCountMax
                    );
                actualMultisampleCount = (MultisampleCount)
                    Math.Min(
                        (int)actualMultisampleCount,
                        (int)GraphicsDevice.Features[DepthBufferFormat].MultisampleCountMax
                    );

                if (
                    GraphicsDevice.Features.HasMultisampleDepthAsSRV == false
                    && GraphicsDevice.Platform != GraphicsPlatform.OpenGL
                    && GraphicsDevice.Platform != GraphicsPlatform.OpenGLES
                )
                {
                    actualMultisampleCount = MultisampleCount.None;
                }

                if (actualMultisampleCount != MSAALevel)
                {
                    logger.Warning(
                        "Multisample count of "
                            + (int)MSAALevel
                            + " samples not supported. Falling back to highest supported sample count of "
                            + (int)actualMultisampleCount
                            + " samples."
                    );
                }

                if (Platform.Type == PlatformType.iOS)
                {
                    actualMultisampleCount = MultisampleCount.None;
                }
            }

            var camera = Context.GetCurrentCamera();

            vrSystem = Services.GetService<VRDeviceSystem>();
            var vrSettings = this.VRSettings;
            if (vrSystem != null && vrSettings != null)
            {
                if (vrSettings.Enabled)
                {
                    var requiredDescs = vrSettings.RequiredApis.ToArray();
                    vrSystem.PreferredApis = requiredDescs.Select(x => x.Api).Distinct().ToArray();

                    var preferredScalings = new Dictionary<VRApi, float>();
                    foreach (var desc in requiredDescs)
                    {
                        if (!preferredScalings.ContainsKey(desc.Api))
                            preferredScalings[desc.Api] = desc.ResolutionScale;
                    }
                    vrSystem.PreferredScalings = preferredScalings;

                    vrSystem.RequireMirror = vrSettings.CopyMirror;
                    vrSystem.MirrorWidth = GraphicsDevice.Presenter.BackBuffer.Width;
                    vrSystem.MirrorHeight = GraphicsDevice.Presenter.BackBuffer.Height;
                    vrSystem.RequestPassthrough = vrSettings.RequestPassthrough;

                    vrSystem.Enabled = true;
                    vrSystem.Visible = true;

                    vrSettings.VRDevice = vrSystem.Device;

                    vrSystem.PreviousUseCustomProjectionMatrix = camera.UseCustomProjectionMatrix;
                    vrSystem.PreviousUseCustomViewMatrix = camera.UseCustomViewMatrix;
                    vrSystem.PreviousCameraProjection = camera.ProjectionMatrix;
                }
                else
                {
                    vrSystem.Enabled = false;
                    vrSystem.Visible = false;

                    vrSettings.VRDevice = null;

                    if (vrSystem.Device != null)
                    {
                        camera.UseCustomViewMatrix = vrSystem.PreviousUseCustomViewMatrix;
                        camera.UseCustomProjectionMatrix =
                            vrSystem.PreviousUseCustomProjectionMatrix;
                        camera.ProjectionMatrix = vrSystem.PreviousCameraProjection;
                    }
                }
            }
        }
    }
}
