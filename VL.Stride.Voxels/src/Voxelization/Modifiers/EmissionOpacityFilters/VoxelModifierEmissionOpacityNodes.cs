using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization.Modifiers.EmissionOpacityFilters
{
    [ProcessNode]
    public abstract class VoxelModifierEmissionOpacityNode<TInstance> : VoxelModifierNode<TInstance>
        where TInstance : VoxelModifierEmissionOpacity, new()
    {
        protected override bool IsImmutable => false;
    }

    /// <summary>
    /// Anti-aliasing modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityAntiAliasing")]
    public class VoxelModifierEmissionOpacityAntiAliasingNode
        : VoxelModifierEmissionOpacityNode<VoxelModifierEmissionOpacityAntiAliasing>
    { }

    /// <summary>
    /// Opacify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityOpacify")]
    public class VoxelModifierEmissionOpacityOpacifyNode
        : VoxelModifierEmissionOpacityNode<VoxelModifierEmissionOpacityOpacify>
    {
        private readonly Cachable<float> _amount;

        public VoxelModifierEmissionOpacityOpacifyNode()
        {
            _amount = new(this, (x, v) => x.Amount = v, 2.0f);
        }

        public void SetAmount(float amount = 2.0f) => _amount.SetValue(amount);
    }

    // <summary>
    /// Solidify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacitySolidify")]
    public class VoxelModifierEmissionOpacitySolidifyNode
        : VoxelModifierEmissionOpacityNode<VoxelModifierEmissionOpacitySolidify>
    { }
}
