using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Anti-aliasing modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityAntiAliasing")]
    public class VoxelModifierEmissionOpacityAntiAliasingNode
        : VoxelNodeImmutable<VoxelModifierEmissionOpacityAntiAliasing> { }

    /// <summary>
    /// Opacify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityOpacify")]
    public class VoxelModifierEmissionOpacityOpacifyNode
        : VoxelNodeImmutable<VoxelModifierEmissionOpacityOpacify> { }

    /// <summary>
    /// Solidify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacitySolidify")]
    public class VoxelModifierEmissionOpacitySolidifyNode
        : VoxelNodeImmutable<VoxelModifierEmissionOpacitySolidify> { }
}
