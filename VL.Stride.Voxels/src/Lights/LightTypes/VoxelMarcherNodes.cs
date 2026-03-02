using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Beam marcher with configurable step count, scale, and diameter.
    /// </summary>
    [ProcessNode(Name = "VoxelMarchBeam")]
    public class VoxelMarchBeamNode : VoxelNodeImmutable<VoxelMarchBeam>
    {
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _stepScale;
        private readonly Cachable<float> _beamDiameter;

        public VoxelMarchBeamNode()
        {
            _steps = new(this, (x, v) => x.Steps = v, 9);
            _stepScale = new(this, (x, v) => x.StepScale = v, 1.0f);
            _beamDiameter = new(this, (x, v) => x.BeamDiameter = v, 1.0f);
        }

        public void SetSteps(int steps = 9) => _steps.SetValue(steps);

        public void SetStepScale(float stepScale = 1.0f) => _stepScale.SetValue(stepScale);

        public void SetBeamDiameter(float beamDiameter = 1.0f) =>
            _beamDiameter.SetValue(beamDiameter);
    }

    /// <summary>
    /// Cone marcher for voxel tracing.
    /// </summary>
    [ProcessNode(Name = "VoxelMarchCone")]
    public class VoxelMarchConeNode : VoxelNodeImmutable<VoxelMarchCone>
    {
        private readonly Cachable<bool> _editMode;
        private readonly Cachable<bool> _fast;
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _stepScale;
        private readonly Cachable<float> _coneRatio;
        private readonly Cachable<float> _startOffset;

        public VoxelMarchConeNode()
        {
            _editMode = new(this, (x, v) => x.EditMode = v, true);
            _fast = new(this, (x, v) => x.Fast = v, false);
            _steps = new(this, (x, v) => x.Steps = v, 9);
            _stepScale = new(this, (x, v) => x.StepScale = v, 1.0f);
            _coneRatio = new(this, (x, v) => x.ConeRatio = v, 1.0f);
            _startOffset = new(this, (x, v) => x.StartOffset = v, 1.0f);
        }

        public void SetEditMode(bool editMode = true) => _editMode.SetValue(editMode);

        public void SetFast(bool fast = false) => _fast.SetValue(fast);

        public void SetSteps(int steps = 9) => _steps.SetValue(steps);

        public void SetStepScale(float stepScale = 1.0f) => _stepScale.SetValue(stepScale);

        public void SetConeRatio(float coneRatio = 1.0f) => _coneRatio.SetValue(coneRatio);

        public void SetStartOffset(float startOffset = 1.0f) => _startOffset.SetValue(startOffset);
    }

    /// <summary>
    /// Cone marcher that traces per mipmap level.
    /// </summary>
    [ProcessNode(Name = "VoxelMarchConePerMipmap")]
    public class VoxelMarchConePerMipmapNode : VoxelNodeImmutable<VoxelMarchConePerMipmap>
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
