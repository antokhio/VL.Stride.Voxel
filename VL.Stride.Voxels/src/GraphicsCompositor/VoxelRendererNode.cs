using Stride.Rendering;
using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.GraphicsCompositor
{
    /// <summary>
    /// Renders voxel volumes using the configured voxel stages.
    /// </summary>
    [ProcessNode(Name = "VoxelRenderer")]
    public class VoxelRendererNode : ProcessNodeBase<VoxelRenderer>
    {
        protected override bool IsImmutable => false;

        private readonly CachableList<RenderStage> _voxelStages;

        public VoxelRendererNode()
        {
            _voxelStages = new(
                this,
                x => x.VoxelStages,
                (x, v) => x.VoxelStages = (List<RenderStage>)v
            );
        }

        public void SetVoxelStages(IReadOnlyList<RenderStage> voxelStages) =>
            _voxelStages.SetValue(voxelStages);
    }
}
