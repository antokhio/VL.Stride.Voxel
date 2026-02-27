using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Clipmap-based voxel storage with configurable resolution and update strategy.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageClipmaps")]
    public class VoxelStorageClipmapsNode : VoxelNodeImmutable<VoxelStorageClipmaps>
    {
        private readonly Cachable<VoxelStorageClipmaps.Resolutions> _clipResolution;
        private readonly Cachable<VoxelStorageClipmaps.UpdateMethods> _updatesPerFrame;
        private readonly Cachable<bool> _downsampleFinerClipMaps;

        public VoxelStorageClipmapsNode()
        {
            _clipResolution = new(
                this,
                (x, v) => x.ClipResolution = v,
                VoxelStorageClipmaps.Resolutions.x128
            );
            _updatesPerFrame = new(
                this,
                (x, v) => x.UpdatesPerFrame = v,
                VoxelStorageClipmaps.UpdateMethods.SingleClipmap
            );
            _downsampleFinerClipMaps = new(this, (x, v) => x.DownsampleFinerClipMaps = v, true);
        }

        private void RebuildIfDirty()
        {
            if (!IsDirty)
                return;
            Rebuild(instance =>
            {
                _clipResolution.ApplyTo(instance);
                _updatesPerFrame.ApplyTo(instance);
                _downsampleFinerClipMaps.ApplyTo(instance);
            });
        }

        public void SetClipResolution(
            VoxelStorageClipmaps.Resolutions clipResolution = VoxelStorageClipmaps.Resolutions.x128
        )
        {
            _clipResolution.SetValue(clipResolution);
            RebuildIfDirty();
        }

        public void SetUpdatesPerFrame(
            VoxelStorageClipmaps.UpdateMethods updatesPerFrame =
                VoxelStorageClipmaps.UpdateMethods.SingleClipmap
        )
        {
            _updatesPerFrame.SetValue(updatesPerFrame);
            RebuildIfDirty();
        }

        public void SetDownsampleFinerClipMaps(bool downsampleFinerClipMaps = true)
        {
            _downsampleFinerClipMaps.SetValue(downsampleFinerClipMaps);
            RebuildIfDirty();
        }
    }
}
