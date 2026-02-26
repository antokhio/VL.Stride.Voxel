using Stride.Rendering;
using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Rendering.Advanced.PipelineProcessors
{
    [ProcessNode(Name = "VoxelPipelineProcessor")]
    public class VoxelPipelineProcessorNode : VoxelGINodeBase<VoxelPipelineProcessor>
    {
        private readonly CachableList<RenderStage> _voxelRenderStage;
        private readonly Cachable<bool> _depthClipping;

        public void SetVoxelRenderStage(List<RenderStage> voxelRenderStage) =>
            _voxelRenderStage.SetValue(voxelRenderStage);

        public void SetDepthClipping(bool depthClipping) => _depthClipping.SetValue(depthClipping);

        public VoxelPipelineProcessorNode()
        {
            _voxelRenderStage = new(this, x => x.VoxelRenderStage);
            _depthClipping = new(this, x => x.DepthClipping, (x, v) => x.DepthClipping = v);
        }
    }
}
