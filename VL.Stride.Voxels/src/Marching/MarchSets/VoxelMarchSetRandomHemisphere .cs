using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Marching.MarchSets
{
    [ProcessNode(Name = "VoxelMarchSetRandomHemisphere")]
    public class VoxelMarchSetRandomHemisphereNode : ProcessNodeBase<VoxelMarchSetRandomHemisphere>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<int> _count;
        private readonly Cachable<bool> _animateNoise;

        public VoxelMarchSetRandomHemisphereNode()
        {
            _marcher = new(this, (x, v) => x.Marcher = v, new VoxelMarchConePerMipmap());
            _count = new(this, (x, v) => x.Count = v, 6);
            _animateNoise = new(this, (x, v) => x.AnimateNoise = v, false);
        }

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetCount(int count = 6) => _count.SetValue(count);

        public void SetAnimateNoise(bool animateNoise = false) =>
            _animateNoise.SetValue(animateNoise);
    }
}
