using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization
{
    /// <summary>
    /// Voxelizes a region of the scene for voxel-based global illumination.
    /// </summary>
    [ProcessNode(Name = "VoxelVolumeComponent")]
    public class VoxelVolumeComponentNode : ProcessNodeBase<VoxelVolumeComponent>
    {
        protected override bool IsImmutable => false;

        private readonly Cachable<bool> _voxelize;
        private readonly Cachable<IVoxelizationMethod> _voxelizationMethod;
        private readonly Cachable<IVoxelStorage> _storage;
        private readonly CachableList<VoxelAttribute> _attributes;
        private readonly Cachable<float> _voxelVolumeSize;
        private readonly Cachable<float> _aproximateVoxelSize;
        private readonly Cachable<bool> _voxelGridSnapping;
        private readonly Cachable<bool> _visualizeVoxels;
        private readonly Cachable<int> _visualizeIndex;
        private readonly Cachable<IVoxelVisualization> _visualization;
        private readonly Cachable<bool> _enabled;

        public VoxelVolumeComponentNode()
        {
            _voxelize = new(this, (x, v) => x.Voxelize = v, true);

            _voxelizationMethod = new(
                this,
                (x, v) => x.VoxelizationMethod = v,
                new VoxelizationMethodDominantAxis()
            );

            _storage = new(this, (x, v) => x.Storage = v, new VoxelStorageClipmaps());
            _attributes = new(
                this,
                x => x.Attributes,
                (x, v) => x.Attributes = (List<VoxelAttribute>)v
            );

            _voxelVolumeSize = new(this, (x, v) => x.VoxelVolumeSize = v, 20f);
            _aproximateVoxelSize = new(this, (x, v) => x.AproximateVoxelSize = v, 0.15f);
            _voxelGridSnapping = new(this, (x, v) => x.VoxelGridSnapping = v, true);
            _visualizeVoxels = new(this, (x, v) => x.VisualizeVoxels = v, false);
            _visualizeIndex = new(this, (x, v) => x.VisualizeIndex = v, 0);
            _visualization = new(this, (x, v) => x.Visualization = v);
            _enabled = new(this, (x, v) => x.Enabled = v, true);
        }

        public void SetVoxelize(bool voxelize = true) => _voxelize.SetValue(voxelize);

        public void SetVoxelizationMethod(IVoxelizationMethod voxelizationMethod) =>
            _voxelizationMethod.SetValue(voxelizationMethod);

        public void SetStorage(IVoxelStorage storage) => _storage.SetValue(storage);

        public void SetAttributes(IReadOnlyList<VoxelAttribute> attributes) =>
            _attributes.SetValue(attributes);

        public void SetVoxelVolumeSize(float voxelVolumeSize = 20f) =>
            _voxelVolumeSize.SetValue(voxelVolumeSize);

        public void SetAproximateVoxelSize(float aproximateVoxelSize = 0.15f) =>
            _aproximateVoxelSize.SetValue(aproximateVoxelSize);

        public void SetVoxelGridSnapping(bool voxelGridSnapping = true) =>
            _voxelGridSnapping.SetValue(voxelGridSnapping);

        public void SetVisualizeVoxels(bool visualizeVoxels = false) =>
            _visualizeVoxels.SetValue(visualizeVoxels);

        public void SetVisualizeIndex(int visualizeIndex = 0) =>
            _visualizeIndex.SetValue(visualizeIndex);

        public void SetVisualization(IVoxelVisualization visualization) =>
            _visualization.SetValue(visualization);

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);
    }
}
