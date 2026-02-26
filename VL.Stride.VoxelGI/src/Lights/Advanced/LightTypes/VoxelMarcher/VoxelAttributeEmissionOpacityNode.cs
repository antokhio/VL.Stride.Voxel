using Stride.Rendering.Voxels;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelAttributeEmissionOpacity")]
    public class VoxelAttributeEmissionOpacityNode : VoxelGINodeBase<VoxelAttributeEmissionOpacity>
    {
        private readonly Cachable<IVoxelLayout> _voxelLayout;
        private readonly CachableList<VoxelModifierEmissionOpacity> _modifiers;
        private readonly Cachable<VoxelAttributeEmissionOpacity.LightFalloffs> _lightFalloff;

        public void SetVoxelLayout(IVoxelLayout voxelLayout) => _voxelLayout.SetValue(voxelLayout);

        public void SetModifiers(IReadOnlyList<VoxelModifierEmissionOpacity> modifiers) =>
            _modifiers.SetValue(modifiers);

        public void SetLightFalloff(
            VoxelAttributeEmissionOpacity.LightFalloffs lightFalloff =
                VoxelAttributeEmissionOpacity.LightFalloffs.Heuristic
        ) => _lightFalloff.SetValue(lightFalloff);

        public VoxelAttributeEmissionOpacityNode()
        {
            _voxelLayout = new(this, x => x.VoxelLayout, (x, v) => x.VoxelLayout = v);
            _modifiers = new(this, x => x.Modifiers);
            _lightFalloff = new(
                this,
                x => x.LightFalloff,
                (x, v) => x.LightFalloff = v,
                VoxelAttributeEmissionOpacity.LightFalloffs.Heuristic
            );
        }
    }
}
