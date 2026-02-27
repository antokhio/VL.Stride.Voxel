using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Anisotropic voxel layout storing directional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutAnisotropic")]
    public class VoxelLayoutAnisotropicNode : VoxelNodeImmutable<VoxelLayoutAnisotropic> { }

    /// <summary>
    /// Paired anisotropic voxel layout storing opposing directional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutAnisotropicPaired")]
    public class VoxelLayoutAnisotropicPairedNode
        : VoxelNodeImmutable<VoxelLayoutAnisotropicPaired> { }

    /// <summary>
    /// Isotropic voxel layout storing omnidirectional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutIsotropic")]
    public class VoxelLayoutIsotropicNode : VoxelNodeImmutable<VoxelLayoutIsotropic> { }
}
