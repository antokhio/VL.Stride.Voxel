using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxelizes a region of the scene for voxel-based global illumination.
    /// </summary>
    [ProcessNode(Name = "VoxelVolumeComponent")]
    public class VoxelVolumeComponentNode : VoxelNodeImmutable<VoxelVolumeComponent>
    {
        private readonly Cachable<bool> _enabled;
        private readonly Cachable<bool> _voxelize;
        private readonly Cachable<IVoxelizationMethod> _voxelizationMethod;
        private readonly Cachable<IVoxelStorage> _storage;
        private readonly Cachable<IReadOnlyList<VoxelAttribute>> _attributes;
        private readonly Cachable<float> _voxelVolumeSize;
        private readonly Cachable<float> _aproximateVoxelSize;
        private readonly Cachable<bool> _voxelGridSnapping;
        private readonly Cachable<bool> _visualizeVoxels;
        private readonly Cachable<int> _visualizeIndex;
        private readonly Cachable<IVoxelVisualization> _visualization;

        public VoxelVolumeComponentNode()
        {
            _enabled = new(this, (x, v) => x.Enabled = v, true);
            _voxelize = new(this, (x, v) => x.Voxelize = v, true);
            _voxelizationMethod = new(this, (x, v) => x.VoxelizationMethod = v);
            _storage = new(this, (x, v) => x.Storage = v);
            _attributes = new(this, (x, v) => x.Attributes = v?.ToList() ?? []);
            _voxelVolumeSize = new(this, (x, v) => x.VoxelVolumeSize = v, 20f);
            _aproximateVoxelSize = new(this, (x, v) => x.AproximateVoxelSize = v, 0.15f);
            _voxelGridSnapping = new(this, (x, v) => x.VoxelGridSnapping = v, true);
            _visualizeVoxels = new(this, (x, v) => x.VisualizeVoxels = v, false);
            _visualizeIndex = new(this, (x, v) => x.VisualizeIndex = v, 0);
            _visualization = new(this, (x, v) => x.Visualization = v);
        }

        private void RebuildIfDirty()
        {
            if (!IsDirty)
                return;
            Rebuild(instance =>
            {
                _enabled.ApplyTo(instance);
                _voxelize.ApplyTo(instance);
                // Preserve Stride defaults for unset reference types
                if (_voxelizationMethod.LastValue is not null)
                    _voxelizationMethod.ApplyTo(instance);
                if (_storage.LastValue is not null)
                    _storage.ApplyTo(instance);
                if (_attributes.LastValue is not null)
                    _attributes.ApplyTo(instance);
                _voxelVolumeSize.ApplyTo(instance);
                _aproximateVoxelSize.ApplyTo(instance);
                _voxelGridSnapping.ApplyTo(instance);
                _visualizeVoxels.ApplyTo(instance);
                _visualizeIndex.ApplyTo(instance);
                if (_visualization.LastValue is not null)
                    _visualization.ApplyTo(instance);
            });
        }

        public void SetEnabled(bool enabled = true)
        {
            _enabled.SetValue(enabled);
            RebuildIfDirty();
        }

        public void SetVoxelize(bool voxelize = true)
        {
            _voxelize.SetValue(voxelize);
            RebuildIfDirty();
        }

        public void SetVoxelizationMethod(IVoxelizationMethod voxelizationMethod)
        {
            _voxelizationMethod.SetValue(voxelizationMethod);
            RebuildIfDirty();
        }

        public void SetStorage(IVoxelStorage storage)
        {
            _storage.SetValue(storage);
            RebuildIfDirty();
        }

        public void SetAttributes(IReadOnlyList<VoxelAttribute> attributes)
        {
            _attributes.SetValue(attributes);
            RebuildIfDirty();
        }

        public void SetVoxelVolumeSize(float voxelVolumeSize = 20f)
        {
            _voxelVolumeSize.SetValue(voxelVolumeSize);
            RebuildIfDirty();
        }

        public void SetAproximateVoxelSize(float aproximateVoxelSize = 0.15f)
        {
            _aproximateVoxelSize.SetValue(aproximateVoxelSize);
            RebuildIfDirty();
        }

        public void SetVoxelGridSnapping(bool voxelGridSnapping = true)
        {
            _voxelGridSnapping.SetValue(voxelGridSnapping);
            RebuildIfDirty();
        }

        public void SetVisualizeVoxels(bool visualizeVoxels = false)
        {
            _visualizeVoxels.SetValue(visualizeVoxels);
            RebuildIfDirty();
        }

        public void SetVisualizeIndex(int visualizeIndex = 0)
        {
            _visualizeIndex.SetValue(visualizeIndex);
            RebuildIfDirty();
        }

        public void SetVisualization(IVoxelVisualization visualization)
        {
            _visualization.SetValue(visualization);
            RebuildIfDirty();
        }
    }
}
