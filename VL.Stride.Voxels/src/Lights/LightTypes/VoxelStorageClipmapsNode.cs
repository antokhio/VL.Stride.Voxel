using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Clipmap-based voxel storage.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageClipmaps")]
    public class VoxelStorageClipmapsNode : VoxelNodeImmutable<VoxelStorageClipmaps> { }
}
