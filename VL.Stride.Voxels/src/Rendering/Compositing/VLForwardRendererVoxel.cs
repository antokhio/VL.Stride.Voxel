using Stride.Rendering;
using Stride.Rendering.Lights;
using Stride.Rendering.Shadows;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Stride.Rendering;
using MeshRenderFeature = Stride.Rendering.MeshRenderFeature;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    /// <summary>
    /// VLForwardRenderer extended with voxel GI support.
    /// Adds VoxelRenderer and VoxelVisualization inputs.
    /// </summary>
    public class VLForwardRendererVoxel : VLForwardRenderer
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

            // Same lookup the base class uses — publicly accessible types
            shadowMapRenderer = Context
                .RenderSystem.RenderFeatures.OfType<MeshRenderFeature>()
                .FirstOrDefault()
                ?.RenderFeatures.OfType<ForwardLightingRenderFeature>()
                .FirstOrDefault()
                ?.ShadowMapRenderer;
        }

        protected override unsafe void CollectCore(RenderContext context)
        {
            // Voxel collection must happen before the base collect
            VoxelRenderer?.Collect(Context, shadowMapRenderer);

            base.CollectCore(context);
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            // Voxel volume rendering before the main draw pass
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

            // Voxel debug visualization overlay (after main rendering)
            if (VoxelVisualization != null)
            {
                VoxelVisualization.VoxelRenderer = VoxelRenderer;
                VoxelVisualization.Draw(drawContext, viewOutputTarget);
            }
        }
    }
}
