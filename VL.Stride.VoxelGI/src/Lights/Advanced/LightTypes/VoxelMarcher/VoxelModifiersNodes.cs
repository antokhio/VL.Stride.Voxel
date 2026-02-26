using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelModifierEmissionOpacityAntiAliasing")]
    public class VoxelModifierEmissionOpacityAntiAliasingNode
        : VoxelGINodeBase<VoxelModifierEmissionOpacityAntiAliasing>
    {
        private readonly Cachable<bool> _enabled;

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);

        public VoxelModifierEmissionOpacityAntiAliasingNode()
        {
            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v, true);
        }
    }

    [ProcessNode(Name = "VoxelModifierEmissionOpacityOpacify")]
    public class VoxelModifierEmissionOpacityOpacifyNode
        : VoxelGINodeBase<VoxelModifierEmissionOpacityOpacify>
    {
        private readonly Cachable<float> _amount;
        private readonly Cachable<bool> _enabled;

        public void SetAmount(float amount = 2.0f) => _amount.SetValue(amount);

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);

        public VoxelModifierEmissionOpacityOpacifyNode()
        {
            _amount = new(this, x => x.Amount, (x, v) => x.Amount = v, 2.0f);
            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v, true);
        }
    }

    [ProcessNode(Name = "VoxelModifierEmissionOpacitySolidify")]
    public class VoxelModifierEmissionOpacitySolidifyNode
        : VoxelGINodeBase<VoxelModifierEmissionOpacitySolidify>
    {
        private readonly Cachable<bool> _enabled;

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);

        public VoxelModifierEmissionOpacitySolidifyNode()
        {
            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v, true);
        }
    }
}
