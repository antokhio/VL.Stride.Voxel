using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel attribute for directional coverage data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeDirectionalCoverage")]
    public class VoxelAttributeDirectionalCoverageNode
        : VoxelNodeMutable<VoxelAttributeDirectionalCoverage>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Voxel attribute for solidity data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeSolidity")]
    public class VoxelAttributeSolidityNode : VoxelNodeMutable<VoxelAttributeSolidity>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
