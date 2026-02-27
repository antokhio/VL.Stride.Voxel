using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Rendering.Images;
using Stride.Rendering.SubsurfaceScattering;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;
using VL.Stride.Rendering;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    /// <summary>
    /// Forward renderer with voxel global illumination support.
    /// Wraps <see cref="VLForwardRendererVoxel"/> as an immutable copy-on-write node.
    /// </summary>
    [ProcessNode(Name = "VLForwardRendererVoxel")]
    public class VLForwardRendererVoxelNode : VoxelNodeImmutable<VLForwardRendererVoxel>
    {
        private readonly Cachable<ClearRenderer> _clear;
        private readonly Cachable<RenderStage> _opaqueRenderStage;
        private readonly Cachable<RenderStage> _transparentRenderStage;
        private readonly Cachable<IReadOnlyList<RenderStage>> _shadowMapRenderStages;
        private readonly Cachable<RenderStage> _gBufferRenderStage;
        private readonly Cachable<PostProcessingEffects> _postEffects;
        private readonly Cachable<LightShafts> _lightShafts;
        private readonly Cachable<VRRendererSettings> _vrSettings;
        private readonly Cachable<ViewportSettings> _viewportSettings;
        private readonly Cachable<SubsurfaceScatteringBlur> _subsurfaceScatteringBlurEffect;
        private readonly Cachable<MultisampleCount> _msaaLevel;
        private readonly Cachable<MSAAResolver> _msaaResolver;
        private readonly Cachable<bool> _bindDepthAsResourceDuringTransparentRendering;
        private readonly Cachable<bool> _bindOpaqueAsResourceDuringTransparentRendering;
        private readonly Cachable<IVoxelRenderer> _voxelRenderer;
        private readonly Cachable<VoxelDebug> _voxelVisualization;
        private readonly Cachable<bool> _enabled;

        public VLForwardRendererVoxelNode()
        {
            _clear = new(this, (x, v) => x.Clear = v);
            _opaqueRenderStage = new(this, (x, v) => x.OpaqueRenderStage = v);
            _transparentRenderStage = new(this, (x, v) => x.TransparentRenderStage = v);
            _shadowMapRenderStages = new(
                this,
                (x, v) =>
                {
                    x.ShadowMapRenderStages.Clear();
                    if (v != null)
                        foreach (var stage in v)
                            if (stage != null)
                                x.ShadowMapRenderStages.Add(stage);
                }
            );
            _gBufferRenderStage = new(this, (x, v) => x.GBufferRenderStage = v);
            _postEffects = new(this, (x, v) => x.PostEffects = v);
            _lightShafts = new(this, (x, v) => x.LightShafts = v);
            _vrSettings = new(this, (x, v) => x.VRSettings = v);
            _viewportSettings = new(this, (x, v) => x.ViewportSettings = v);
            _subsurfaceScatteringBlurEffect = new(
                this,
                (x, v) => x.SubsurfaceScatteringBlurEffect = v
            );
            _msaaLevel = new(this, (x, v) => x.MSAALevel = v);
            _msaaResolver = new(
                this,
                (x, v) =>
                {
                    if (v != null)
                    {
                        var s = x.MSAAResolver;
                        s.FilterType = v.FilterType;
                        s.FilterRadius = v.FilterRadius;
                    }
                }
            );
            _bindDepthAsResourceDuringTransparentRendering = new(
                this,
                (x, v) => x.BindDepthAsResourceDuringTransparentRendering = v
            );
            _bindOpaqueAsResourceDuringTransparentRendering = new(
                this,
                (x, v) => x.BindOpaqueAsResourceDuringTransparentRendering = v
            );
            _voxelRenderer = new(this, (x, v) => x.VoxelRenderer = v);
            _voxelVisualization = new(this, (x, v) => x.VoxelVisualization = v);
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        public void SetClear(ClearRenderer clear) => _clear.SetValue(clear);

        public void SetOpaqueRenderStage(RenderStage opaqueRenderStage) =>
            _opaqueRenderStage.SetValue(opaqueRenderStage);

        public void SetTransparentRenderStage(RenderStage transparentRenderStage) =>
            _transparentRenderStage.SetValue(transparentRenderStage);

        public void SetShadowMapRenderStages(IReadOnlyList<RenderStage> shadowMapRenderStages) =>
            _shadowMapRenderStages.SetValue(shadowMapRenderStages);

        public void SetGBufferRenderStage(RenderStage gBufferRenderStage) =>
            _gBufferRenderStage.SetValue(gBufferRenderStage);

        public void SetPostEffects(PostProcessingEffects postEffects) =>
            _postEffects.SetValue(postEffects);

        public void SetLightShafts(LightShafts lightShafts) => _lightShafts.SetValue(lightShafts);

        public void SetVRSettings(VRRendererSettings vrSettings) =>
            _vrSettings.SetValue(vrSettings);

        public void SetViewportSettings(ViewportSettings viewportSettings) =>
            _viewportSettings.SetValue(viewportSettings);

        public void SetSubsurfaceScatteringBlurEffect(
            SubsurfaceScatteringBlur subsurfaceScatteringBlurEffect
        ) => _subsurfaceScatteringBlurEffect.SetValue(subsurfaceScatteringBlurEffect);

        public void SetMSAALevel(MultisampleCount msaaLevel = MultisampleCount.None) =>
            _msaaLevel.SetValue(msaaLevel);

        public void SetMSAAResolver(MSAAResolver msaaResolver) =>
            _msaaResolver.SetValue(msaaResolver);

        public void SetBindDepthAsResourceDuringTransparentRendering(
            bool bindDepthAsResourceDuringTransparentRendering = false
        ) =>
            _bindDepthAsResourceDuringTransparentRendering.SetValue(
                bindDepthAsResourceDuringTransparentRendering
            );

        public void SetBindOpaqueAsResourceDuringTransparentRendering(
            bool bindOpaqueAsResourceDuringTransparentRendering = false
        ) =>
            _bindOpaqueAsResourceDuringTransparentRendering.SetValue(
                bindOpaqueAsResourceDuringTransparentRendering
            );

        public void SetVoxelRenderer(IVoxelRenderer voxelRenderer) =>
            _voxelRenderer.SetValue(voxelRenderer);

        public void SetVoxelVisualization(VoxelDebug voxelVisualization) =>
            _voxelVisualization.SetValue(voxelVisualization);

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);

        public void Update()
        {
            if (IsDirty)
            {
                Rebuild(x =>
                {
                    _clear.ApplyTo(x);
                    _opaqueRenderStage.ApplyTo(x);
                    _transparentRenderStage.ApplyTo(x);
                    _shadowMapRenderStages.ApplyTo(x);
                    _gBufferRenderStage.ApplyTo(x);
                    _postEffects.ApplyTo(x);
                    _lightShafts.ApplyTo(x);
                    _vrSettings.ApplyTo(x);
                    _viewportSettings.ApplyTo(x);
                    _subsurfaceScatteringBlurEffect.ApplyTo(x);
                    _msaaLevel.ApplyTo(x);
                    _msaaResolver.ApplyTo(x);
                    _bindDepthAsResourceDuringTransparentRendering.ApplyTo(x);
                    _bindOpaqueAsResourceDuringTransparentRendering.ApplyTo(x);
                    _voxelRenderer.ApplyTo(x);
                    _voxelVisualization.ApplyTo(x);
                    _enabled.ApplyTo(x);
                });
            }
        }
    }
}
