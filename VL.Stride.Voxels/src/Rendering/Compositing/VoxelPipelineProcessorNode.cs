using Stride.Rendering;
using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    /// <summary>
    /// Processes the voxel pipeline, assigning a render stage and optional depth clipping.
    /// </summary>
    [ProcessNode(Name = "VoxelPipelineProcessor")]
    public class VoxelPipelineProcessorNode : VoxelNodeMutable<VoxelPipelineProcessor>
    {
        private readonly CachableList<RenderStage> _voxelRenderStage;
        private readonly Cachable<bool> _depthClipping;

        public VoxelPipelineProcessorNode()
        {
            _voxelRenderStage = new(
                this,
                x => x.VoxelRenderStage,
                (x, v) => x.VoxelRenderStage = (List<RenderStage>)v
            );
            _depthClipping = new(this, x => x.DepthClipping, (x, v) => x.DepthClipping = v);
        }

        public void SetVoxelRenderStage(IReadOnlyList<RenderStage> voxelRenderStage) =>
            _voxelRenderStage.SetValue(voxelRenderStage);

        public void SetDepthClipping(bool depthClipping) => _depthClipping.SetValue(depthClipping);
    }
}
