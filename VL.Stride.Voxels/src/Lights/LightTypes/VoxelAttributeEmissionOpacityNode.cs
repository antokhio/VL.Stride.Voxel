using Stride.Rendering.Voxels;
using VL.Core.Import;
using static Stride.Rendering.Voxels.VoxelAttributeEmissionOpacity;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel attribute for emission and opacity data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeEmissionOpacity")]
    public class VoxelAttributeEmissionOpacityNode
        : VoxelNodeImmutable<VoxelAttributeEmissionOpacity>
    {
        private readonly Cachable<IVoxelLayout> _voxelLayout;
        private readonly Cachable<IReadOnlyList<VoxelModifierEmissionOpacity>> _modifiers;
        private readonly Cachable<LightFalloffs> _lightFalloff;

        public VoxelAttributeEmissionOpacityNode()
        {
            _voxelLayout = new(this, (x, v) => x.VoxelLayout = v);
            _modifiers = new(this, (x, v) => x.Modifiers = v?.ToList() ?? []);
            _lightFalloff = new(this, (x, v) => x.LightFalloff = v, LightFalloffs.Heuristic);
        }

        public void SetVoxelLayout(IVoxelLayout voxelLayout) => _voxelLayout.SetValue(voxelLayout);

        public void SetModifiers(IReadOnlyList<VoxelModifierEmissionOpacity> modifiers) =>
            _modifiers.SetValue(modifiers);

        public void SetLightFalloff(LightFalloffs lightFalloff = LightFalloffs.Heuristic) =>
            _lightFalloff.SetValue(lightFalloff);
    }
}
