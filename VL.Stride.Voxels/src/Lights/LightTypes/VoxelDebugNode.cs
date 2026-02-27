using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Debug visualization for voxel data.
    /// </summary>
    [ProcessNode(Name = "VoxelDebug")]
    public class VoxelDebugNode : VoxelNodeMutable<VoxelDebug>
    {
        private readonly Cachable<bool> _enabled;

        public VoxelDebugNode()
        {
            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v);
        }

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);
    }
}
