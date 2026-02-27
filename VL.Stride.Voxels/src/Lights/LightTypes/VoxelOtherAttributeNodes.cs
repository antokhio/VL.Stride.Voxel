using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel attribute for directional coverage data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeDirectionalCoverage")]
    public class VoxelAttributeDirectionalCoverageNode
        : VoxelNodeImmutable<VoxelAttributeDirectionalCoverage> { }

    /// <summary>
    /// Voxel attribute for solidity data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeSolidity")]
    public class VoxelAttributeSolidityNode : VoxelNodeImmutable<VoxelAttributeSolidity> { }
}
