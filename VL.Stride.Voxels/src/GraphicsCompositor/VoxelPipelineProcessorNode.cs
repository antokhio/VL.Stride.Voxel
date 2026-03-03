using Stride.Rendering;
using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.GraphicsCompositor
{
    /// <summary>
    /// Processes the voxel pipeline, assigning a render stage and optional depth clipping.
    /// </summary>
    [ProcessNode(Name = "VoxelPipelineProcessor")]
    public class VoxelPipelineProcessorNode : ProcessNodeBase<VoxelPipelineProcessor>
    {
        protected override bool IsImmutable => false;

        private readonly CachableList<RenderStage> _voxelRenderStage;
        private readonly Cachable<bool> _depthClipping;

        public VoxelPipelineProcessorNode()
        {
            _voxelRenderStage = new(
                this,
                x => x.VoxelRenderStage,
                (x, v) => x.VoxelRenderStage = (List<RenderStage>)v
            );

            _depthClipping = new(this, (x, v) => x.DepthClipping = v, false);
        }

        public void SetVoxelRenderStage(IReadOnlyList<RenderStage> voxelRenderStage) =>
            _voxelRenderStage.SetValue(voxelRenderStage);

        public void SetDepthClipping(bool depthClipping = false) =>
            _depthClipping.SetValue(depthClipping);
    }
}
