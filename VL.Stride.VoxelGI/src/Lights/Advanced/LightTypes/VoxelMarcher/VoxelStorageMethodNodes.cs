using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelStorageMethodIndirect")]
    public class VoxelStorageMethodIndirectNode : VoxelGINodeBase<VoxelStorageMethodIndirect>
    {
        private readonly Cachable<IVoxelFragmentPacker> _tempStorageFormat;
        private readonly Cachable<IVoxelBufferWriter> _filter;

        public void SetTempStorageFormat(IVoxelFragmentPacker tempStorageFormat) =>
            _tempStorageFormat.SetValue(tempStorageFormat);

        public void SetFilter(IVoxelBufferWriter filter) => _filter.SetValue(filter);

        public VoxelStorageMethodIndirectNode()
        {
            _tempStorageFormat = new(
                this,
                x => x.TempStorageFormat,
                (x, v) => x.TempStorageFormat = v
            );
            _filter = new(this, x => x.Filter, (x, v) => x.Filter = v);
        }
    }
}
