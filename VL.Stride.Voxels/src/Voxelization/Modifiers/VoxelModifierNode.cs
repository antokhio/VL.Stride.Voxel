using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization.Modifiers
{
    [ProcessNode]
    public abstract class VoxelModifierNode<TInstance> : ProcessNodeBase<TInstance>
        where TInstance : VoxelModifier, new()
    {
        private readonly Cachable<bool> _enabled;

        public VoxelModifierNode()
        {
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        [Fragment(Order = int.MaxValue)]
        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);
    }
}
