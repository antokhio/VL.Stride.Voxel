using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Packs voxel fragment data as float16.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloat16")]
    public class VoxelFragmentPackFloat16Node : VoxelNodeMutable<VoxelFragmentPackFloat16>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Packs voxel fragment data as float32.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloat32")]
    public class VoxelFragmentPackFloat32Node : VoxelNodeMutable<VoxelFragmentPackFloat32>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Packs voxel fragment data as R11G11B10 float.
    /// </summary>
    [ProcessNode(Name = "VoxelFragmentPackFloatR11G11B10")]
    public class VoxelFragmentPackFloatR11G11B10Node
        : VoxelNodeMutable<VoxelFragmentPackFloatR11G11B10>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
