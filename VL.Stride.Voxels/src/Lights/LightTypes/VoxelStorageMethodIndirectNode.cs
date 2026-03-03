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
            _tempStorageFormat = new(
                this,
                (x, v) => x.TempStorageFormat = v ?? new VoxelFragmentPackFloatR11G11B10(),
                new VoxelFragmentPackFloatR11G11B10()
            );
            _filter = new(
                this,
                (x, v) => x.Filter = v ?? new VoxelBufferWriteMax(),
                new VoxelBufferWriteMax()
            );
        }

        protected override void OnBuildInstance(VoxelStorageMethodIndirect instance)
        {
            instance.TempStorageFormat ??= new VoxelFragmentPackFloatR11G11B10();
            instance.Filter ??= new VoxelBufferWriteMax();
        }

        public void SetTempStorageFormat(IVoxelFragmentPacker tempStorageFormat) =>
            _tempStorageFormat.SetValue(tempStorageFormat);

        public void SetFilter(IVoxelBufferWriter filter) => _filter.SetValue(filter);
    }
}
