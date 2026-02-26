using Stride.Core;
using Stride.Rendering;
using Stride.Rendering.Lights;
using Stride.Rendering.Shadows;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Stride.Rendering;
using MeshRenderFeature = Stride.Rendering.MeshRenderFeature;

namespace VL.Stride.VoxelGI.Rendering
{
    [Display("VL VoxelGI Forward Renderer")]
    public class VLVoxelGIForwardRenderer : VLForwardRenderer
    {
        private IShadowMapRenderer shadowMapRenderer;

        /// <summary>
        /// The voxel renderer.
        /// </summary>
        public IVoxelRenderer VoxelRenderer { get; set; }

        /// <summary>
        /// Voxel debug visualization.
        /// </summary>
        public VoxelDebug VoxelVisualization { get; set; }

        protected override void InitializeCore()
        {
            base.InitializeCore();

            // We need to retrieve the shadowMapRenderer ourselves since it's private in the base class.
            shadowMapRenderer = Context
                .RenderSystem.RenderFeatures.OfType<MeshRenderFeature>()
                .FirstOrDefault()
                ?.RenderFeatures.OfType<ForwardLightingRenderFeature>()
                .FirstOrDefault()
                ?.ShadowMapRenderer;
        }

        protected override void CollectCore(RenderContext context)
        {
            // Collect voxels before base collection
            VoxelRenderer?.Collect(context, shadowMapRenderer);

            base.CollectCore(context);
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            // Draw voxels before base drawing
            if (VoxelRenderer != null)
            {
                using (drawContext.PushRenderTargetsAndRestore())
                {
                    VoxelRenderer.Draw(drawContext, shadowMapRenderer);
                }
            }

            base.DrawCore(context, drawContext);
        }

        protected override void DrawView(
            RenderContext context,
            RenderDrawContext drawContext,
            int eyeIndex,
            int eyeCount,
            bool renderPostFX = true
        )
        {
            base.DrawView(context, drawContext, eyeIndex, eyeCount, renderPostFX);

            // Draw Voxel Debug after everything else if enabled
            if (VoxelVisualization != null)
            {
                VoxelVisualization.VoxelRenderer = VoxelRenderer;
                VoxelVisualization.Draw(drawContext, viewOutputTarget);
            }
        }
    }
}
