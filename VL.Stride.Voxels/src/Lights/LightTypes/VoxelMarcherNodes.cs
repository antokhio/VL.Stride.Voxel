using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Beam marcher with configurable step count, scale, and diameter.
    /// </summary>
    [ProcessNode(Name = "VoxelMarchBeam")]
    public class VoxelMarchBeamNode : VoxelNodeMutable<VoxelMarchBeam>
    {
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _stepScale;
        private readonly Cachable<float> _beamDiameter;

        public VoxelMarchBeamNode()
        {
            _steps = new(this, x => x.Steps, (x, v) => x.Steps = v, 9);
            _stepScale = new(this, x => x.StepScale, (x, v) => x.StepScale = v, 1.0f);
            _beamDiameter = new(this, x => x.BeamDiameter, (x, v) => x.BeamDiameter = v, 1.0f);
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
    public class VoxelMarchConeNode : VoxelNodeMutable<VoxelMarchCone>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }

    /// <summary>
    /// Cone marcher that traces per mipmap level.
    /// </summary>
    [ProcessNode(Name = "VoxelMarchConePerMipmap")]
    public class VoxelMarchConePerMipmapNode : VoxelNodeMutable<VoxelMarchConePerMipmap>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
