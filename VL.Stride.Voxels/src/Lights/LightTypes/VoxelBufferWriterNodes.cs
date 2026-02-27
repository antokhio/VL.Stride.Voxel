using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Writes voxel buffer data using direct assignment.
    /// </summary>
    [ProcessNode(Name = "VoxelBufferWriteAssign")]
    public class VoxelBufferWriteAssignNode : VoxelNodeMutable<VoxelBufferWriteAssign>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Writes voxel buffer data using max blending.
    /// </summary>
    [ProcessNode(Name = "VoxelBufferWriteMax")]
    public class VoxelBufferWriteMaxNode : VoxelNodeMutable<VoxelBufferWriteMax>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
