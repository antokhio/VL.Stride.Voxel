using Stride.Rendering.Voxels.VoxelGI;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Light
{
    [ProcessNode(Name = "LightVoxelRenderer")]
    public class LightVoxelRendererNode : ProcessNodeBase<LightVoxelRenderer>
    {
        protected override bool IsImmutable => false;
    }
}
