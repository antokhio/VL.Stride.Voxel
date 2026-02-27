using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Anti-aliasing modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityAntiAliasing")]
    public class VoxelModifierEmissionOpacityAntiAliasingNode
        : VoxelNodeImmutable<VoxelModifierEmissionOpacityAntiAliasing>
    {
        private readonly Cachable<bool> _enabled;

        public VoxelModifierEmissionOpacityAntiAliasingNode()
        {
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        public void SetEnabled(bool enabled = true)
        {
            _enabled.SetValue(enabled);
            if (IsDirty)
                Rebuild(instance => _enabled.ApplyTo(instance));
        }
    }

    /// <summary>
    /// Opacify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacityOpacify")]
    public class VoxelModifierEmissionOpacityOpacifyNode
        : VoxelNodeImmutable<VoxelModifierEmissionOpacityOpacify>
    {
        private readonly Cachable<bool> _enabled;

        public VoxelModifierEmissionOpacityOpacifyNode()
        {
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        public void SetEnabled(bool enabled = true)
        {
            _enabled.SetValue(enabled);
            if (IsDirty)
                Rebuild(instance => _enabled.ApplyTo(instance));
        }
    }

    /// <summary>
    /// Solidify modifier for voxel emission and opacity.
    /// </summary>
    [ProcessNode(Name = "VoxelModifierEmissionOpacitySolidify")]
    public class VoxelModifierEmissionOpacitySolidifyNode
        : VoxelNodeImmutable<VoxelModifierEmissionOpacitySolidify>
    {
        private readonly Cachable<bool> _enabled;

        public VoxelModifierEmissionOpacitySolidifyNode()
        {
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        public void SetEnabled(bool enabled = true)
        {
            _enabled.SetValue(enabled);
            if (IsDirty)
                Rebuild(instance => _enabled.ApplyTo(instance));
        }
    }
}
