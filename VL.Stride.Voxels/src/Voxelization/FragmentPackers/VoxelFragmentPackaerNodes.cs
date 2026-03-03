using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization.FragmentPackers
{
    /// <summary>
    /// Packs voxel fragment data as float16.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloat16")]
    public class VoxelFragmentPackFloat16Node : ProcessNodeBase<VoxelFragmentPackFloat16> { }

    /// <summary>
    /// Packs voxel fragment data as float32.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloat32")]
    public class VoxelFragmentPackFloat32Node : ProcessNodeBase<VoxelFragmentPackFloat32> { }

    /// <summary>
    /// Packs voxel fragment data as R11G11B10 float.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloatR11G11B10")]
    public class VoxelFragmentPackFloatR11G11B10Node
        : ProcessNodeBase<VoxelFragmentPackFloatR11G11B10> { }
}
