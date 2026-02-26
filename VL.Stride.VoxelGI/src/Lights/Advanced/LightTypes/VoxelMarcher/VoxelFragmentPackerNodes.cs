using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelFragmentPackFloat16")]
    public class VoxelFragmentPackFloat16Node : VoxelGINodeBase<VoxelFragmentPackFloat16> { }

    [ProcessNode(Name = "VoxelFragmentPackFloat32")]
    public class VoxelFragmentPackFloat32Node : VoxelGINodeBase<VoxelFragmentPackFloat32> { }

    [ProcessNode(Name = "VoxelFragmentPackFloatR11G11B10")]
    public class VoxelFragmentPackFloatR11G11B10Node
        : VoxelGINodeBase<VoxelFragmentPackFloatR11G11B10> { }
}
