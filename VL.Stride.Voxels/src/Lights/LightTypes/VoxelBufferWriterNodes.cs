using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Writes voxel buffer data using direct assignment.
    /// </summary>
    [ProcessNode(Name = "VoxelBufferWriteAssign")]
    public class VoxelBufferWriteAssignNode : VoxelNodeImmutable<VoxelBufferWriteAssign> { }

    /// <summary>
    /// Writes voxel buffer data using max blending.
    /// </summary>
    [ProcessNode(Name = "VoxelBufferWriteMax")]
    public class VoxelBufferWriteMaxNode : VoxelNodeImmutable<VoxelBufferWriteMax> { }
}
