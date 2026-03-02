using Stride.Rendering.Voxels;
using VL.Core.Import;
using static Stride.Rendering.Voxels.VoxelLayoutBase;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Anisotropic voxel layout storing directional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutAnisotropic")]
    public class VoxelLayoutAnisotropicNode : VoxelNodeImmutable<VoxelLayoutAnisotropic>
    {
        private readonly Cachable<float> _maxBrightness;
        private readonly Cachable<IVoxelStorageMethod> _storageMethod;
        private readonly Cachable<StorageFormats> _storageFormat;

        public VoxelLayoutAnisotropicNode()
        {
            _maxBrightness = new(this, (x, v) => x.maxBrightness = v, 10.0f);
            _storageMethod = new(this, (x, v) => x.StorageMethod = v);
            _storageFormat = new(this, (x, v) => x.StorageFormat = v, StorageFormats.RGBA16F);
        }

        public void SetMaxBrightness(float maxBrightness = 10.0f) =>
            _maxBrightness.SetValue(maxBrightness);

        public void SetStorageMethod(IVoxelStorageMethod storageMethod) =>
            _storageMethod.SetValue(storageMethod);

        public void SetStorageFormat(StorageFormats storageFormat = StorageFormats.RGBA16F) =>
            _storageFormat.SetValue(storageFormat);
    }

    /// <summary>
    /// Paired anisotropic voxel layout storing opposing directional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutAnisotropicPaired")]
    public class VoxelLayoutAnisotropicPairedNode : VoxelNodeImmutable<VoxelLayoutAnisotropicPaired>
    {
        private readonly Cachable<float> _maxBrightness;
        private readonly Cachable<IVoxelStorageMethod> _storageMethod;
        private readonly Cachable<StorageFormats> _storageFormat;

        public VoxelLayoutAnisotropicPairedNode()
        {
            _maxBrightness = new(this, (x, v) => x.maxBrightness = v, 10.0f);
            _storageMethod = new(this, (x, v) => x.StorageMethod = v);
            _storageFormat = new(this, (x, v) => x.StorageFormat = v, StorageFormats.RGBA16F);
        }

        public void SetMaxBrightness(float maxBrightness = 10.0f) =>
            _maxBrightness.SetValue(maxBrightness);

        public void SetStorageMethod(IVoxelStorageMethod storageMethod) =>
            _storageMethod.SetValue(storageMethod);

        public void SetStorageFormat(StorageFormats storageFormat = StorageFormats.RGBA16F) =>
            _storageFormat.SetValue(storageFormat);
    }

    /// <summary>
    /// Isotropic voxel layout storing omnidirectional data.
    /// </summary>
    [ProcessNode(Name = "VoxelLayoutIsotropic")]
    public class VoxelLayoutIsotropicNode : VoxelNodeImmutable<VoxelLayoutIsotropic>
    {
        private readonly Cachable<float> _maxBrightness;
        private readonly Cachable<IVoxelStorageMethod> _storageMethod;
        private readonly Cachable<StorageFormats> _storageFormat;

        public VoxelLayoutIsotropicNode()
        {
            _maxBrightness = new(this, (x, v) => x.maxBrightness = v, 10.0f);
            _storageMethod = new(this, (x, v) => x.StorageMethod = v);
            _storageFormat = new(this, (x, v) => x.StorageFormat = v, StorageFormats.RGBA16F);
        }

        public void SetMaxBrightness(float maxBrightness = 10.0f) =>
            _maxBrightness.SetValue(maxBrightness);

        public void SetStorageMethod(IVoxelStorageMethod storageMethod) =>
            _storageMethod.SetValue(storageMethod);

        public void SetStorageFormat(StorageFormats storageFormat = StorageFormats.RGBA16F) =>
            _storageFormat.SetValue(storageFormat);
    }
}
