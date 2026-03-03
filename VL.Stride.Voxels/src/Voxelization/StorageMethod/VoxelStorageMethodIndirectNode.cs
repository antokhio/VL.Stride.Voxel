using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization.StorageMethod
{
    /// <summary>
    /// Indirect storage method for voxel data.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageMethodIndirect")]
    public class VoxelStorageMethodIndirectNode : ProcessNodeBase<VoxelStorageMethodIndirect>
    {
        private readonly Cachable<IVoxelFragmentPacker> _tempStorageFormat;
        private readonly Cachable<IVoxelBufferWriter> _filter;

        public VoxelStorageMethodIndirectNode()
        {
            _tempStorageFormat = new(
                this,
                (x, v) => x.TempStorageFormat = v,
                new VoxelFragmentPackFloatR11G11B10()
            );

            // Added default VoxelBufferWriteMax to match Stride
            _filter = new(this, (x, v) => x.Filter = v, new VoxelBufferWriteMax());
        }

        public void SetTempStorageFormat(IVoxelFragmentPacker tempStorageFormat) =>
            _tempStorageFormat.SetValue(tempStorageFormat);

        public void SetFilter(IVoxelBufferWriter filter) => _filter.SetValue(filter);
    }
}
