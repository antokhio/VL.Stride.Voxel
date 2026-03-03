using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.GraphicsCompositor
{
    /// <summary>
    /// Debug visualization for voxel data.
    /// </summary>
    [ProcessNode(Name = "VoxelDebug")]
    public class VoxelDebugNode : ProcessNodeBase<VoxelDebug>
    {
        protected override bool IsImmutable => false;

        private readonly Cachable<bool> _enabled;

        public VoxelDebugNode()
        {
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);
    }
}
