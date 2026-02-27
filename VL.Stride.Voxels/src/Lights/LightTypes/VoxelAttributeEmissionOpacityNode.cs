using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel attribute for emission and opacity data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeEmissionOpacity")]
    public class VoxelAttributeEmissionOpacityNode : VoxelNodeMutable<VoxelAttributeEmissionOpacity>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
