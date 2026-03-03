using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Marching.MarchSets
{
    [ProcessNode]
    public abstract class VoxelMarchSetBaseNode<TInstance> : ProcessNodeBase<TInstance>
        where TInstance : VoxelMarchSetBase, new()
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<float> _offset;

        protected VoxelMarchSetBaseNode()
        {
            _marcher = new(this, (x, v) => x.Marcher = v, new VoxelMarchConePerMipmap());
            _offset = new(this, (x, v) => x.Offset = v, 1.0f);
        }

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetOffset(float offset = 1.0f) => _offset.SetValue(offset);
    }

    [ProcessNode(Name = "VoxelMarchSetHemisphere6")]
    public class VoxelMarchSetHemisphere6Node : VoxelMarchSetBaseNode<VoxelMarchSetHemisphere6> { }

    [ProcessNode(Name = "VoxelMarchSetHemisphere12")]
    public class VoxelMarchSetHemisphere12Node
        : VoxelMarchSetBaseNode<VoxelMarchSetHemisphere12> { }
}
