using Stride.Core.Diagnostics;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxelizes a region of the scene for voxel-based global illumination.
    /// Ensures there is always at least one attribute to avoid Stride voxel GI crashes.
    /// </summary>
    [ProcessNode(Name = "VoxelVolumeComponent")]
    public class VoxelVolumeComponentNode : VoxelNodeImmutable<VoxelVolumeComponent>
    {
        private static readonly Logger Log = GlobalLogger.GetLogger(
            nameof(VoxelVolumeComponentNode)
        );

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
        private readonly Cachable<bool> _enabled;

        public VoxelVolumeComponentNode()
        {
            _voxelize = new(this, (x, v) => x.Voxelize = v, true);
            _voxelizationMethod = new(this, (x, v) => x.VoxelizationMethod = v);
            _storage = new(this, (x, v) => x.Storage = v);

            var defaultAttributes = CreateDefaultAttributes();

            _attributes = new(
                this,
                (x, v) =>
                {
                    var attrs = EnsureValidAttributes(v);
                    x.Attributes = attrs;

                    Log.Warning(
                        $"[VoxelVolumeComponentNode] SetAttributes instance={x.GetHashCode()} count={attrs.Count} "
                            + $"types=[{string.Join(", ", attrs.Select(a => a?.GetType().Name ?? "null"))}]"
                    );
                },
                defaultAttributes
            );

            _voxelVolumeSize = new(this, (x, v) => x.VoxelVolumeSize = v, 20f);
            _aproximateVoxelSize = new(this, (x, v) => x.AproximateVoxelSize = v, 0.15f);
            _voxelGridSnapping = new(this, (x, v) => x.VoxelGridSnapping = v, true);
            _visualizeVoxels = new(this, (x, v) => x.VisualizeVoxels = v, false);
            _visualizeIndex = new(this, (x, v) => x.VisualizeIndex = v, 0);
            _visualization = new(this, (x, v) => x.Visualization = v);
            _enabled = new(this, (x, v) => x.Enabled = v, true);

            // Seed the initial Output instance immediately (before first immutable rebuild tick).
            Output.Attributes = EnsureValidAttributes(defaultAttributes);
        }

        protected override void OnBuildInstance(VoxelVolumeComponent instance)
        {
            instance.Attributes = EnsureValidAttributes(instance.Attributes);

            Log.Warning(
                $"[VoxelVolumeComponentNode] OnBuildInstance instance={instance.GetHashCode()} count={instance.Attributes.Count}"
            );
        }

        private static List<VoxelAttribute> CreateDefaultAttributes() =>
            new() { new VoxelAttributeEmissionOpacity(), new VoxelAttributeDirectionalCoverage() };

        private static List<VoxelAttribute> EnsureValidAttributes(
            IReadOnlyList<VoxelAttribute> input
        )
        {
            var attrs = input?.Where(a => a != null).ToList() ?? new List<VoxelAttribute>();

            if (attrs.Count == 0)
                attrs = CreateDefaultAttributes();

            foreach (var attribute in attrs)
                EnsureAttributeIntegrity(attribute);

            return attrs;
        }

        private static void EnsureAttributeIntegrity(VoxelAttribute attribute)
        {
            if (attribute is not VoxelAttributeEmissionOpacity emission)
                return;

            emission.VoxelLayout ??= new VoxelLayoutIsotropic();

            switch (emission.VoxelLayout)
            {
                case VoxelLayoutIsotropic isotropic:
                    isotropic.StorageMethod ??= new VoxelStorageMethodIndirect();
                    break;
                case VoxelLayoutAnisotropic anisotropic:
                    anisotropic.StorageMethod ??= new VoxelStorageMethodIndirect();
                    break;
                case VoxelLayoutAnisotropicPaired paired:
                    paired.StorageMethod ??= new VoxelStorageMethodIndirect();
                    break;
            }
        }

        public void SetVoxelize(bool voxelize = true) => _voxelize.SetValue(voxelize);

        public void SetVoxelizationMethod(IVoxelizationMethod voxelizationMethod) =>
            _voxelizationMethod.SetValue(voxelizationMethod);

        public void SetStorage(IVoxelStorage storage) => _storage.SetValue(storage);

        public void SetAttributes(IReadOnlyList<VoxelAttribute> attributes) =>
            _attributes.SetValue(EnsureValidAttributes(attributes));

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
