using Stride.Rendering.Voxels.VoxelGI;
using VL.Core.Import;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    /// <summary>
    /// Renders voxel-based lighting.
    /// </summary>
    [ProcessNode(Name = "LightVoxelRenderer")]
    public class LightVoxelRendererNode : VoxelNodeMutable<LightVoxelRenderer>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
