using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization.VoxelStorage
{
    /// <summary>
    /// Clipmap-based voxel storage with configurable resolution and update strategy.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageClipmaps")]
    public class VoxelStorageClipmapsNode : ProcessNodeBase<VoxelStorageClipmaps>
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

        public void SetClipResolution(
            VoxelStorageClipmaps.Resolutions clipResolution = VoxelStorageClipmaps.Resolutions.x128
        ) => _clipResolution.SetValue(clipResolution);

        public void SetUpdatesPerFrame(
            VoxelStorageClipmaps.UpdateMethods updatesPerFrame =
                VoxelStorageClipmaps.UpdateMethods.SingleClipmap
        ) => _updatesPerFrame.SetValue(updatesPerFrame);

        public void SetDownsampleFinerClipMaps(bool downsampleFinerClipMaps = true) =>
            _downsampleFinerClipMaps.SetValue(downsampleFinerClipMaps);
    }
}
