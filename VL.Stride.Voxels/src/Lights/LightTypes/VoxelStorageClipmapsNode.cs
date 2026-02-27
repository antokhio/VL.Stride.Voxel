using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Clipmap-based voxel storage.
    /// </summary>
    [ProcessNode(Name = "VoxelStorageClipmaps")]
    public class VoxelStorageClipmapsNode : VoxelNodeMutable<VoxelStorageClipmaps>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
