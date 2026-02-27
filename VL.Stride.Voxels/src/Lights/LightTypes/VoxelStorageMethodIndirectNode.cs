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

        private void RebuildIfDirty()
        {
            if (!IsDirty)
                return;
            Rebuild(instance =>
            {
                // Preserve Stride defaults (VoxelFragmentPackFloatR11G11B10, VoxelBufferWriteMax) if unset
                if (_tempStorageFormat.LastValue is not null)
                    _tempStorageFormat.ApplyTo(instance);
                if (_filter.LastValue is not null)
                    _filter.ApplyTo(instance);
            });
        }

        public void SetTempStorageFormat(IVoxelFragmentPacker tempStorageFormat)
        {
            _tempStorageFormat.SetValue(tempStorageFormat);
            RebuildIfDirty();
        }

        public void SetFilter(IVoxelBufferWriter filter)
        {
            _filter.SetValue(filter);
            RebuildIfDirty();
        }
    }
}
