using Stride.Rendering;
using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    /// <summary>
    /// Renders voxel volumes using the configured voxel stages.
    /// </summary>
    [ProcessNode(Name = "VoxelRenderer")]
    public class VoxelRendererNode : VoxelNodeMutable<VoxelRenderer>
    {
        private readonly CachableList<RenderStage> _voxelStages;

        public VoxelRendererNode()
        {
            _voxelStages = new(this, x => x.VoxelStages);
        }

        public void SetVoxelStages(IReadOnlyList<RenderStage> voxelStages) =>
            _voxelStages.SetValue(voxelStages);
    }
}
