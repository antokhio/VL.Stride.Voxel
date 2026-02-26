using Stride.Graphics;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Lights.Advanced.LightTypes.VoxelMarcher
{
    [ProcessNode(Name = "VoxelMarchSetHemisphere6")]
    public class VoxelMarchSetHemisphere6Node : VoxelGINodeBase<VoxelMarchSetHemisphere6>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<float> _offset;

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetOffset(float offset) => _offset.SetValue(offset);

        public VoxelMarchSetHemisphere6Node()
        {
            _marcher = new(this, x => x.Marcher, (x, v) => x.Marcher = v);
            _offset = new(this, x => x.Offset, (x, v) => x.Offset = v, 1.0f);
        }
    }

    [ProcessNode(Name = "VoxelMarchSetHemisphere12")]
    public class VoxelMarchSetHemisphere12Node : VoxelGINodeBase<VoxelMarchSetHemisphere12>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<float> _offset;

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetOffset(float offset) => _offset.SetValue(offset);

        public VoxelMarchSetHemisphere12Node()
        {
            _marcher = new(this, x => x.Marcher, (x, v) => x.Marcher = v);
            _offset = new(this, x => x.Offset, (x, v) => x.Offset = v, 1.0f);
        }
    }

    [ProcessNode(Name = "VoxelMarchSetRandomHemisphere")]
    public class VoxelMarchSetRandomHemisphereNode : VoxelGINodeBase<VoxelMarchSetRandomHemisphere>
    {
        private readonly Cachable<IVoxelMarchMethod> _marcher;
        private readonly Cachable<int> _count;
        private readonly Cachable<bool> _animateNoise;

        public void SetMarcher(IVoxelMarchMethod marcher) => _marcher.SetValue(marcher);

        public void SetCount(int count) => _count.SetValue(count);

        public void SetAnimateNoise(bool animateNoise) => _animateNoise.SetValue(animateNoise);

        public VoxelMarchSetRandomHemisphereNode()
        {
            _marcher = new(this, x => x.Marcher, (x, v) => x.Marcher = v);
            _count = new(this, x => x.Count, (x, v) => x.Count = v, 6);
            _animateNoise = new(this, x => x.AnimateNoise, (x, v) => x.AnimateNoise = v, false);
        }
    }

    [ProcessNode(Name = "VoxelMarchBeam")]
    public class VoxelMarchBeamNode : VoxelGINodeBase<VoxelMarchBeam>
    {
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _stepScale;
        private readonly Cachable<float> _beamDiameter;

        public void SetSteps(int steps) => _steps.SetValue(steps);

        public void SetStepScale(float stepScale) => _stepScale.SetValue(stepScale);

        public void SetBeamDiameter(float beamDiameter) => _beamDiameter.SetValue(beamDiameter);

        public VoxelMarchBeamNode()
        {
            _steps = new(this, x => x.Steps, (x, v) => x.Steps = v, 9);
            _stepScale = new(this, x => x.StepScale, (x, v) => x.StepScale = v, 1.0f);
            _beamDiameter = new(this, x => x.BeamDiameter, (x, v) => x.BeamDiameter = v, 1.0f);
        }
    }

    [ProcessNode(Name = "VoxelMarchCone")]
    public class VoxelMarchConeNode : VoxelGINodeBase<VoxelMarchCone>
    {
        private readonly Cachable<bool> _editMode;
        private readonly Cachable<bool> _fast;
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _stepScale;
        private readonly Cachable<float> _coneRatio;
        private readonly Cachable<float> _startOffset;

        public void SetEditMode(bool editMode = true) => _editMode.SetValue(editMode);

        public void SetFast(bool fast = false) => _fast.SetValue(fast);

        public void SetSteps(int steps = 9) => _steps.SetValue(steps);

        public void SetStepScale(float stepScale = 1.0f) => _stepScale.SetValue(stepScale);

        public void SetConeRatio(float coneRatio = 1.0f) => _coneRatio.SetValue(coneRatio);

        public void SetStartOffset(float startOffset = 1.0f) => _startOffset.SetValue(startOffset);

        public VoxelMarchConeNode()
        {
            _editMode = new(this, x => x.EditMode, (x, v) => x.EditMode = v, true);
            _fast = new(this, x => x.Fast, (x, v) => x.Fast = v, false);
            _steps = new(this, x => x.Steps, (x, v) => x.Steps = v, 9);
            _stepScale = new(this, x => x.StepScale, (x, v) => x.StepScale = v, 1.0f);
            _coneRatio = new(this, x => x.ConeRatio, (x, v) => x.ConeRatio = v, 1.0f);
            _startOffset = new(this, x => x.StartOffset, (x, v) => x.StartOffset = v, 1.0f);
        }
    }

    [ProcessNode(Name = "VoxelMarchConePerMipmap")]
    public class VoxelMarchConePerMipmapNode : VoxelGINodeBase<VoxelMarchConePerMipmap>
    {
        private readonly Cachable<int> _steps;
        private readonly Cachable<float> _coneRatio;
        private readonly Cachable<float> _startOffset;

        public void SetSteps(int steps) => _steps.SetValue(steps);

        public void SetConeRatio(float coneRatio) => _coneRatio.SetValue(coneRatio);

        public void SetStartOffset(float startOffset) => _startOffset.SetValue(startOffset);

        public VoxelMarchConePerMipmapNode()
        {
            _steps = new(this, x => x.Steps, (x, v) => x.Steps = v);
            _coneRatio = new(this, x => x.ConeRatio, (x, v) => x.ConeRatio = v);
            _startOffset = new(this, x => x.StartOffset, (x, v) => x.StartOffset = v);
        }
    }

    [ProcessNode(Name = "VoxelDebug")]
    public class VoxelDebugNode : VoxelGINodeBase<VoxelDebug>
    {
        private readonly Cachable<bool> _enabled;

        public void SetEnabled(bool enabled = true) => _enabled.SetValue(enabled);

        public VoxelDebugNode()
        {
            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v, true);
        }
    }

    [ProcessNode(Name = "VoxelVolumeComponent")]
    public class VoxelVolumeComponentNode : VoxelGINodeBase<VoxelVolumeComponent>
    {
        private readonly Cachable<bool> _voxelize;
        private readonly Cachable<IVoxelizationMethod> _voxelizationMethod;
        private readonly Cachable<IVoxelStorage> _storage;
        private readonly CachableList<VoxelAttribute> _attributes;
        private readonly Cachable<float> _voxelVolumeSize;
        private readonly Cachable<float> _aproximateVoxelSize;
        private readonly Cachable<bool> _voxelGridSnapping;
        private readonly Cachable<bool> _visualizeVoxels;
        private readonly Cachable<int> _visualizeIndex;
        private readonly Cachable<IVoxelVisualization> _visualization;
        private readonly Cachable<bool> _enabled;

        public void SetVoxelize(bool voxelize) => _voxelize.SetValue(voxelize);

        public void SetVoxelizationMethod(IVoxelizationMethod voxelizationMethod) =>
            _voxelizationMethod.SetValue(voxelizationMethod);

        public void SetStorage(IVoxelStorage storage) => _storage.SetValue(storage);

        public void SetAttributes(IReadOnlyList<VoxelAttribute> attributes) =>
            _attributes.SetValue(attributes);

        public void SetVoxelVolumeSize(float voxelVolumeSize) =>
            _voxelVolumeSize.SetValue(voxelVolumeSize);

        public void SetAproximateVoxelSize(float aproximateVoxelSize) =>
            _aproximateVoxelSize.SetValue(aproximateVoxelSize);

        public void SetVoxelGridSnapping(bool voxelGridSnapping) =>
            _voxelGridSnapping.SetValue(voxelGridSnapping);

        public void SetVisualizeVoxels(bool visualizeVoxels) =>
            _visualizeVoxels.SetValue(visualizeVoxels);

        public void SetVisualizeIndex(int visualizeIndex) =>
            _visualizeIndex.SetValue(visualizeIndex);

        public void SetVisualization(IVoxelVisualization visualization) =>
            _visualization.SetValue(visualization);

        public void SetEnabled(bool enabled) => _enabled.SetValue(enabled);

        public VoxelVolumeComponentNode()
        {
            _voxelize = new(this, x => x.Voxelize, (x, v) => x.Voxelize = v, true);
            _voxelizationMethod = new(
                this,
                x => x.VoxelizationMethod,
                (x, v) => x.VoxelizationMethod = v
            );
            _storage = new(this, x => x.Storage, (x, v) => x.Storage = v);
            _attributes = new(this, x => x.Attributes);
            _voxelVolumeSize = new(
                this,
                x => x.VoxelVolumeSize,
                (x, v) => x.VoxelVolumeSize = v,
                20f
            );
            _aproximateVoxelSize = new(
                this,
                x => x.AproximateVoxelSize,
                (x, v) => x.AproximateVoxelSize = v,
                0.15f
            );
            _voxelGridSnapping = new(
                this,
                x => x.VoxelGridSnapping,
                (x, v) => x.VoxelGridSnapping = v,
                true
            );
            _visualizeVoxels = new(
                this,
                x => x.VisualizeVoxels,
                (x, v) => x.VisualizeVoxels = v,
                false
            );
            _visualizeIndex = new(this, x => x.VisualizeIndex, (x, v) => x.VisualizeIndex = v, 0);
            _visualization = new(this, x => x.Visualization, (x, v) => x.Visualization = v);
            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v, true);
        }
    }

    [ProcessNode(Name = "VoxelizationMethodSingleAxis")]
    public class VoxelizationMethodSingleAxisNode : VoxelGINodeBase<VoxelizationMethodSingleAxis>
    {
        private readonly Cachable<VoxelizationMethodSingleAxis.Axis> _voxelizationAxis;

        public void SetVoxelizationAxis(VoxelizationMethodSingleAxis.Axis voxelizationAxis) =>
            _voxelizationAxis.SetValue(voxelizationAxis);

        public VoxelizationMethodSingleAxisNode()
        {
            _voxelizationAxis = new(
                this,
                x => x.VoxelizationAxis,
                (x, v) => x.VoxelizationAxis = v,
                VoxelizationMethodSingleAxis.Axis.Y
            );
        }
    }

    [ProcessNode(Name = "VoxelizationMethodDominantAxis")]
    public class VoxelizationMethodDominantAxisNode
        : VoxelGINodeBase<VoxelizationMethodDominantAxis>
    {
        private readonly Cachable<MultisampleCount> _multisampleCount;

        public void SetMultisampleCount(MultisampleCount multisampleCount) =>
            _multisampleCount.SetValue(multisampleCount);

        public VoxelizationMethodDominantAxisNode()
        {
            _multisampleCount = new(
                this,
                x => x.MultisampleCount,
                (x, v) => x.MultisampleCount = v,
                MultisampleCount.X8
            );
        }
    }

    [ProcessNode(Name = "VoxelizationMethodTriAxis")]
    public class VoxelizationMethodTriAxisNode : VoxelGINodeBase<VoxelizationMethodTriAxis>
    {
        private readonly Cachable<MultisampleCount> _multisampleCount;

        public void SetMultisampleCount(MultisampleCount multisampleCount) =>
            _multisampleCount.SetValue(multisampleCount);

        public VoxelizationMethodTriAxisNode()
        {
            _multisampleCount = new(
                this,
                x => x.MultisampleCount,
                (x, v) => x.MultisampleCount = v,
                MultisampleCount.X8
            );
        }
    }

    [ProcessNode(Name = "VoxelStorageClipmaps")]
    public class VoxelStorageClipmapsNode : VoxelGINodeBase<VoxelStorageClipmaps>
    {
        private readonly Cachable<VoxelStorageClipmaps.Resolutions> _clipResolution;
        private readonly Cachable<VoxelStorageClipmaps.UpdateMethods> _updatesPerFrame;
        private readonly Cachable<bool> _downsampleFinerClipMaps;

        public void SetClipResolution(
            VoxelStorageClipmaps.Resolutions clipResolution = VoxelStorageClipmaps.Resolutions.x128
        ) => _clipResolution.SetValue(clipResolution);

        public void SetUpdatesPerFrame(
            VoxelStorageClipmaps.UpdateMethods updatesPerFrame =
                VoxelStorageClipmaps.UpdateMethods.SingleClipmap
        ) => _updatesPerFrame.SetValue(updatesPerFrame);

        public void SetDownsampleFinerClipMaps(bool downsampleFinerClipMaps = true) =>
            _downsampleFinerClipMaps.SetValue(downsampleFinerClipMaps);

        public VoxelStorageClipmapsNode()
        {
            _clipResolution = new(
                this,
                x => x.ClipResolution,
                (x, v) => x.ClipResolution = v,
                VoxelStorageClipmaps.Resolutions.x128
            );
            _updatesPerFrame = new(
                this,
                x => x.UpdatesPerFrame,
                (x, v) => x.UpdatesPerFrame = v,
                VoxelStorageClipmaps.UpdateMethods.SingleClipmap
            );
            _downsampleFinerClipMaps = new(
                this,
                x => x.DownsampleFinerClipMaps,
                (x, v) => x.DownsampleFinerClipMaps = v,
                true
            );
        }
    }
}
