using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Hemisphere march set with 6 directions.
    /// </summary>
    [ProcessNode(Name = "MarchSetHemisphere6")]
    public class MarchSetHemisphere6 : VoxelNodeMutable<VoxelMarchSetHemisphere6>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<float> _offset;

        public MarchSetHemisphere6()
        {
            _marcher = new(this, x => x.Marcher, (x, v) => x.Marcher = v);
            _offset = new(this, x => x.Offset, (x, v) => x.Offset = v);
        }

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetOffset(float offset) => _offset.SetValue(offset);
    }

    /// <summary>
    /// Hemisphere march set with 12 directions.
    /// </summary>
    [ProcessNode(Name = "MarchSetHemisphere12")]
    public class MarchSetHemisphere12 : VoxelNodeMutable<VoxelMarchSetHemisphere12>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<float> _offset;

        public MarchSetHemisphere12()
        {
            _marcher = new(this, x => x.Marcher, (x, v) => x.Marcher = v);
            _offset = new(this, x => x.Offset, (x, v) => x.Offset = v);
        }

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetOffset(float offset) => _offset.SetValue(offset);
    }

    /// <summary>
    /// Randomized hemisphere march set with configurable sample count and noise animation.
    /// </summary>
    [ProcessNode(Name = "MarchSetRandomHemisphere")]
    public class MarchSetRandomHemisphere : VoxelNodeMutable<VoxelMarchSetRandomHemisphere>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<int> _count;
        private readonly Cachable<bool> _animateNoise;

        public MarchSetRandomHemisphere()
        {
            _marcher = new(this, x => x.Marcher, (x, v) => x.Marcher = v);
            _count = new(this, x => x.Count, (x, v) => x.Count = v, 6);
            _animateNoise = new(this, x => x.AnimateNoise, (x, v) => x.AnimateNoise = v, false);
        }

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetCount(int count = 6) => _count.SetValue(count);

        public void SetAnimateNoise(bool animateNoise = false) =>
            _animateNoise.SetValue(animateNoise);
    }
}
