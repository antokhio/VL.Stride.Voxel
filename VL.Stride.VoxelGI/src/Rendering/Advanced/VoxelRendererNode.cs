using Stride.Rendering;
using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Rendering.Advanced
{
    [ProcessNode(Name = "VoxelRenderer")]
    public class VoxelRendererNode : VoxelGINodeBase<VoxelRenderer>
    {
        private readonly CachableList<RenderStage> _voxelStages;

        public void SetVoxelStages(IReadOnlyList<RenderStage> voxelStages) =>
            _voxelStages.SetValue(voxelStages);

        public VoxelRendererNode()
        {
            _voxelStages = new CachableList<RenderStage>(this, x => x.VoxelStages);
        }
    }
}
