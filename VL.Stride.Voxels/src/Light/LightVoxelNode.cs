using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.VoxelGI;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Light
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
            _volume = new(this, (x, v) => x.Volume = v);
            _attributeIndex = new(this, (x, v) => x.AttributeIndex = v, 0);

            _diffuseMarcher = new(
                this,
                (x, v) => x.DiffuseMarcher = v,
                new VoxelMarchSetHemisphere6(new VoxelMarchConePerMipmap())
            );

            _specularMarcher = new(
                this,
                (x, v) => x.SpecularMarcher = v,
                new VoxelMarchCone(30, 0.5f, 1.0f)
            );

            _bounceIntensityScale = new(this, (x, v) => x.BounceIntensityScale = v, 0f);
            _specularIntensityScale = new(this, (x, v) => x.SpecularIntensityScale = v, 0f);
        }

        public void SetVolume(VoxelVolumeComponent volume = null) => _volume.SetValue(volume);

        public void SetAttributeIndex(int attributeIndex = 0) =>
            _attributeIndex.SetValue(attributeIndex);

        public void SetDiffuseMarcher(IVoxelMarchSet diffuseMarcher) =>
            _diffuseMarcher.SetValue(diffuseMarcher);

        public void SetSpecularMarcher(IVoxelMarchMethod specularMarcher) =>
            _specularMarcher.SetValue(specularMarcher);

        public void SetBounceIntensityScale(float bounceIntensityScale = 0f) =>
            _bounceIntensityScale.SetValue(bounceIntensityScale);

        public void SetSpecularIntensityScale(float specularIntensityScale = 0f) =>
            _specularIntensityScale.SetValue(specularIntensityScale);
    }
}
