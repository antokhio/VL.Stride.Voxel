using Stride.Rendering.Voxels;
using VL.Core.Import;
using static Stride.Rendering.Voxels.VoxelAttributeEmissionOpacity;

namespace VL.Stride.Rendering.Voxels.Voxelization.Attributes
{
    [ProcessNode]
    public abstract class VoxelAttributeNode<TInstance> : ProcessNodeBase<TInstance>
        where TInstance : VoxelAttribute, new()
    { }

    /// <summary>
    /// Voxel attribute for emission and opacity data.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeEmissionOpacity")]
    public class VoxelAttributeEmissionOpacityNode
        : VoxelAttributeNode<VoxelAttributeEmissionOpacity>
    {
        protected override bool IsImmutable => false;

        private readonly Cachable<IVoxelLayout> _voxelLayout;
        private readonly CachableList<VoxelModifierEmissionOpacity> _modifiers;
        private readonly Cachable<LightFalloffs> _lightFalloff;

        public VoxelAttributeEmissionOpacityNode()
        {
            _voxelLayout = new(this, (x, v) => x.VoxelLayout = v, new VoxelLayoutIsotropic());

            _modifiers = new(
                this,
                x => x.Modifiers,
                (x, v) => x.Modifiers = (List<VoxelModifierEmissionOpacity>)v
            );

            _lightFalloff = new(this, (x, v) => x.LightFalloff = v, LightFalloffs.Heuristic);
        }

        public void SetVoxelLayout(IVoxelLayout voxelLayout) => _voxelLayout.SetValue(voxelLayout);

        public void SetModifiers(IReadOnlyList<VoxelModifierEmissionOpacity> modifiers) =>
            _modifiers.SetValue(modifiers);

        public void SetLightFalloff(LightFalloffs lightFalloff = LightFalloffs.Heuristic) =>
            _lightFalloff.SetValue(lightFalloff);
    }

    /// <summary>
    /// Voxel attribute for directional coverage.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeDirectionalCoverage")]
    public class VoxelAttributeDirectionalCoverageNode
        : VoxelAttributeNode<VoxelAttributeDirectionalCoverage>
    { }

    /// <summary>
    /// Voxel attribute for solidity.
    /// </summary>
    [ProcessNode(Name = "VoxelAttributeSolidity")]
    public class VoxelAttributeSolidityNode : VoxelAttributeNode<VoxelAttributeSolidity> { }
}
