using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Anisotropic voxel layout storing directional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutAnisotropic")]
    public class VoxelLayoutAnisotropicNode : VoxelNodeMutable<VoxelLayoutAnisotropic>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Paired anisotropic voxel layout storing opposing directional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutAnisotropicPaired")]
    public class VoxelLayoutAnisotropicPairedNode : VoxelNodeMutable<VoxelLayoutAnisotropicPaired>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Isotropic voxel layout storing omnidirectional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutIsotropic")]
    public class VoxelLayoutIsotropicNode : VoxelNodeMutable<VoxelLayoutIsotropic>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
