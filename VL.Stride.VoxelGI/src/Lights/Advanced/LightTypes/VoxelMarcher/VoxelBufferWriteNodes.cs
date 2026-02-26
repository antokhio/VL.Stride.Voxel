using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelBufferWriteAssign")]
    public class VoxelBufferWriteAssignNode : VoxelGINodeBase<VoxelBufferWriteAssign> { }

    [ProcessNode(Name = "VoxelBufferWriteMax")]
    public class VoxelBufferWriteMaxNode : VoxelGINodeBase<VoxelBufferWriteMax> { }
}
