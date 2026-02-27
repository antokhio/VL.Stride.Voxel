using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Anti-aliasing modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityAntiAliasing")]
    public class VoxelModifierEmissionOpacityAntiAliasingNode
        : VoxelNodeMutable<VoxelModifierEmissionOpacityAntiAliasing>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Opacify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityOpacify")]
    public class VoxelModifierEmissionOpacityOpacifyNode
        : VoxelNodeMutable<VoxelModifierEmissionOpacityOpacify>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Solidify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacitySolidify")]
    public class VoxelModifierEmissionOpacitySolidifyNode
        : VoxelNodeMutable<VoxelModifierEmissionOpacitySolidify>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
