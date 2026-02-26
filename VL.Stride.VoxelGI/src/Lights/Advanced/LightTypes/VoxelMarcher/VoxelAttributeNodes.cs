using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelAttributeDirectionalCoverage")]
    public class VoxelAttributeDirectionalCoverageNode
        : VoxelGINodeBase<VoxelAttributeDirectionalCoverage> { }

    [ProcessNode(Name = "VoxelAttributeSolidity")]
    public class VoxelAttributeSolidityNode : VoxelGINodeBase<VoxelAttributeSolidity> { }
}
