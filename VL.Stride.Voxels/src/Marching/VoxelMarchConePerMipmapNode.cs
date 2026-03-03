using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Marching
{
    /// <summary>
    /// Cone marcher that traces per mipmap level.
    /// </summary>
    [ProcessNode(Name = "VoxelMarchConePerMipmap")]
    public class VoxelMarchConePerMipmapNode : ProcessNodeBase<VoxelMarchConePerMipmap>
    {
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _coneRatio;
        private readonly Cachable<float> _startOffset;

        public VoxelMarchConePerMipmapNode()
        {
            _steps = new(this, (x, v) => x.Steps = v, 7);
            _coneRatio = new(this, (x, v) => x.ConeRatio = v, 1f);
            _startOffset = new(this, (x, v) => x.StartOffset = v, 0.5f);
        }

        public void SetSteps(int steps = 7) => _steps.SetValue(steps);

        public void SetConeRatio(float coneRatio = 1f) => _coneRatio.SetValue(coneRatio);

        public void SetStartOffset(float startOffset = 0.5f) => _startOffset.SetValue(startOffset);
    }
}
