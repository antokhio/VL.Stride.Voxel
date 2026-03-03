using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.VoxelGI;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel-based environment light using diffuse and specular marchers.
    /// </summary>
    [ProcessNode(Name = "LightVoxel")]
    public class LightVoxelNode : ProcessNodeBase<LightVoxel>
    {
        private readonly Cachable<VoxelVolumeComponent> _volume;
        private readonly Cachable<int> _attributeIndex;
        private readonly Cachable<IVoxelMarchSet> _diffuseMarcher;
        private readonly Cachable<IVoxelMarchMethod> _specularMarcher;
        private readonly Cachable<float> _bounceIntensityScale;
        private readonly Cachable<float> _specularIntensityScale;

        public LightVoxelNode()
        {
            _volume = new(this, x => x.Volume, (x, v) => x.Volume = v);
            _attributeIndex = new(this, x => x.AttributeIndex, (x, v) => x.AttributeIndex = v);
            _diffuseMarcher = new(this, x => x.DiffuseMarcher, (x, v) => x.DiffuseMarcher = v);
            _specularMarcher = new(this, x => x.SpecularMarcher, (x, v) => x.SpecularMarcher = v);
            _bounceIntensityScale = new(
                this,
                x => x.BounceIntensityScale,
                (x, v) => x.BounceIntensityScale = v
            );
            _specularIntensityScale = new(
                this,
                x => x.SpecularIntensityScale,
                (x, v) => x.SpecularIntensityScale = v
            );
        }

        public void SetVolume(VoxelVolumeComponent volume) => _volume.SetValue(volume);

        public void SetAttributeIndex(int attributeIndex) =>
            _attributeIndex.SetValue(attributeIndex);

        public void SetDiffuseMarcher(IVoxelMarchSet diffuseMarcher) =>
            _diffuseMarcher.SetValue(diffuseMarcher);

        public void SetSpecularMarcher(IVoxelMarchMethod specularMarcher) =>
            _specularMarcher.SetValue(specularMarcher);

        public void SetBounceIntensityScale(float bounceIntensityScale) =>
            _bounceIntensityScale.SetValue(bounceIntensityScale);

        public void SetSpecularIntensityScale(float specularIntensityScale) =>
            _specularIntensityScale.SetValue(specularIntensityScale);
    }
}
