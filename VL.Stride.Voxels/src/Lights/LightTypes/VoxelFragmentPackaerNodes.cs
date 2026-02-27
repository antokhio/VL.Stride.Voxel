using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Packs voxel fragment data as float16.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloat16")]
    public class VoxelFragmentPackFloat16Node : VoxelNodeImmutable<VoxelFragmentPackFloat16> { }

    /// <summary>
    /// Packs voxel fragment data as float32.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloat32")]
    public class VoxelFragmentPackFloat32Node : VoxelNodeImmutable<VoxelFragmentPackFloat32> { }

    /// <summary>
    /// Packs voxel fragment data as R11G11B10 float.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloatR11G11B10")]
    public class VoxelFragmentPackFloatR11G11B10Node
        : VoxelNodeImmutable<VoxelFragmentPackFloatR11G11B10> { }
}
