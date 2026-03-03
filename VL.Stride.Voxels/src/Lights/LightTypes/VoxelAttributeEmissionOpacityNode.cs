using Stride.Core.Diagnostics;
using Stride.Rendering.Voxels;
using VL.Core.Import;
using static Stride.Rendering.Voxels.VoxelAttributeEmissionOpacity;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    [ProcessNode(Name = "VoxelAttributeEmissionOpacity")]
    public class VoxelAttributeEmissionOpacityNode
        : VoxelNodeImmutable<VoxelAttributeEmissionOpacity>
    {
        private static readonly Logger Log = GlobalLogger.GetLogger(
            nameof(VoxelAttributeEmissionOpacityNode)
        );

        private readonly Cachable<IVoxelLayout> _voxelLayout;
        private readonly Cachable<IReadOnlyList<VoxelModifierEmissionOpacity>> _modifiers;
        private readonly Cachable<LightFalloffs> _lightFalloff;

        public VoxelAttributeEmissionOpacityNode()
        {
            _voxelLayout = new(this, (x, v) => x.VoxelLayout = EnsureLayout(v), EnsureLayout(null));
            _modifiers = new(
                this,
                (x, v) => x.Modifiers = v?.ToList() ?? new List<VoxelModifierEmissionOpacity>()
            );
            _lightFalloff = new(this, (x, v) => x.LightFalloff = v, LightFalloffs.Heuristic);
        }

        protected override void OnBuildInstance(VoxelAttributeEmissionOpacity instance)
        {
            instance.VoxelLayout = EnsureLayout(instance.VoxelLayout);
            instance.Modifiers ??= new List<VoxelModifierEmissionOpacity>();

            var storageMethodName = instance.VoxelLayout switch
            {
                VoxelLayoutIsotropic l => l.StorageMethod?.GetType().Name,
                VoxelLayoutAnisotropic l => l.StorageMethod?.GetType().Name,
                VoxelLayoutAnisotropicPaired l => l.StorageMethod?.GetType().Name,
                _ => null,
            };

            Log.Warning(
                $"[VoxelAttributeEmissionOpacityNode] Build instance={instance.GetHashCode()} layout={instance.VoxelLayout.GetType().Name} storageMethod={storageMethodName ?? "null"}"
            );
        }

        private static IVoxelLayout EnsureLayout(IVoxelLayout layout)
        {
            layout ??= new VoxelLayoutIsotropic();
            EnsureStorageMethod(layout);
            return layout;
        }

        private static void EnsureStorageMethod(IVoxelLayout layout)
        {
            switch (layout)
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

        public void SetVoxelLayout(IVoxelLayout voxelLayout) =>
            _voxelLayout.SetValue(EnsureLayout(voxelLayout));

        public void SetModifiers(IReadOnlyList<VoxelModifierEmissionOpacity> modifiers) =>
            _modifiers.SetValue(modifiers);

        public void SetLightFalloff(LightFalloffs lightFalloff = LightFalloffs.Heuristic) =>
            _lightFalloff.SetValue(lightFalloff);
    }
}
