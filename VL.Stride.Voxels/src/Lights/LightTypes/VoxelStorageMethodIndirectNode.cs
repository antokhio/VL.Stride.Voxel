using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Indirect storage method for voxel data.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageMethodIndirect")]
    public class VoxelStorageMethodIndirectNode : VoxelNodeMutable<VoxelStorageMethodIndirect>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
