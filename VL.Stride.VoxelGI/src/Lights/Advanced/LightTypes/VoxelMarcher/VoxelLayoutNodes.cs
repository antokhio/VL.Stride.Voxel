using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelLayoutAnisotropic")]
    public class VoxelLayoutAnisotropicNode : VoxelGINodeBase<VoxelLayoutAnisotropic>
    {
        private readonly Cachable<IVoxelStorageMethod> _storageMethod;
        private readonly Cachable<VoxelLayoutBase.StorageFormats> _storageFormat;
        private readonly Cachable<float> _maxBrightness;

        public void SetStorageMethod(IVoxelStorageMethod storageMethod) =>
            _storageMethod.SetValue(storageMethod);

        public void SetStorageFormat(
            VoxelLayoutBase.StorageFormats storageFormat = VoxelLayoutBase.StorageFormats.RGBA16F
        ) => _storageFormat.SetValue(storageFormat);

        public void SetMaxBrightness(float maxBrightness = 10f) =>
            _maxBrightness.SetValue(maxBrightness);

        public VoxelLayoutAnisotropicNode()
        {
            _storageMethod = new(this, x => x.StorageMethod, (x, v) => x.StorageMethod = v);
            _storageFormat = new(
                this,
                x => x.StorageFormat,
                (x, v) => x.StorageFormat = v,
                VoxelLayoutBase.StorageFormats.RGBA16F
            );
            _maxBrightness = new(this, x => x.maxBrightness, (x, v) => x.maxBrightness = v, 10f);
        }
    }

    [ProcessNode(Name = "VoxelLayoutAnisotropicPaired")]
    public class VoxelLayoutAnisotropicPairedNode : VoxelGINodeBase<VoxelLayoutAnisotropicPaired>
    {
        private readonly Cachable<IVoxelStorageMethod> _storageMethod;
        private readonly Cachable<VoxelLayoutBase.StorageFormats> _storageFormat;
        private readonly Cachable<float> _maxBrightness;

        public void SetStorageMethod(IVoxelStorageMethod storageMethod) =>
            _storageMethod.SetValue(storageMethod);

        public void SetStorageFormat(
            VoxelLayoutBase.StorageFormats storageFormat = VoxelLayoutBase.StorageFormats.RGBA16F
        ) => _storageFormat.SetValue(storageFormat);

        public void SetMaxBrightness(float maxBrightness = 10f) =>
            _maxBrightness.SetValue(maxBrightness);

        public VoxelLayoutAnisotropicPairedNode()
        {
            _storageMethod = new(this, x => x.StorageMethod, (x, v) => x.StorageMethod = v);
            _storageFormat = new(
                this,
                x => x.StorageFormat,
                (x, v) => x.StorageFormat = v,
                VoxelLayoutBase.StorageFormats.RGBA16F
            );
            _maxBrightness = new(this, x => x.maxBrightness, (x, v) => x.maxBrightness = v, 10f);
        }
    }

    [ProcessNode(Name = "VoxelLayoutIsotropic")]
    public class VoxelLayoutIsotropicNode : VoxelGINodeBase<VoxelLayoutIsotropic>
    {
        private readonly Cachable<IVoxelStorageMethod> _storageMethod;
        private readonly Cachable<VoxelLayoutBase.StorageFormats> _storageFormat;
        private readonly Cachable<float> _maxBrightness;

        public void SetStorageMethod(IVoxelStorageMethod storageMethod) =>
            _storageMethod.SetValue(storageMethod);

        public void SetStorageFormat(
            VoxelLayoutBase.StorageFormats storageFormat = VoxelLayoutBase.StorageFormats.RGBA16F
        ) => _storageFormat.SetValue(storageFormat);

        public void SetMaxBrightness(float maxBrightness = 10f) =>
            _maxBrightness.SetValue(maxBrightness);

        public VoxelLayoutIsotropicNode()
        {
            _storageMethod = new(this, x => x.StorageMethod, (x, v) => x.StorageMethod = v);
            _storageFormat = new(
                this,
                x => x.StorageFormat,
                (x, v) => x.StorageFormat = v,
                VoxelLayoutBase.StorageFormats.RGBA16F
            );
            _maxBrightness = new(this, x => x.maxBrightness, (x, v) => x.maxBrightness = v, 10f);
        }
    }
}
