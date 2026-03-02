using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Indirect storage method for voxel data.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageMethodIndirect")]
    public class VoxelStorageMethodIndirectNode : VoxelNodeImmutable<VoxelStorageMethodIndirect>
    {
        private readonly Cachable<IVoxelFragmentPacker> _tempStorageFormat;
        private readonly Cachable<IVoxelBufferWriter> _filter;

        public VoxelStorageMethodIndirectNode()
        {
            _tempStorageFormat = new(this, (x, v) => x.TempStorageFormat = v);
            _filter = new(this, (x, v) => x.Filter = v);
        }

        public void SetTempStorageFormat(IVoxelFragmentPacker tempStorageFormat) =>
            _tempStorageFormat.SetValue(tempStorageFormat);

        public void SetFilter(IVoxelBufferWriter filter) => _filter.SetValue(filter);
    }
}
