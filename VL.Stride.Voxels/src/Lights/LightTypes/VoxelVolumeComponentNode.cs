using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel volume component for configuring voxelization.
    /// </summary>
    [ProcessNode(Name = "VoxelVolumeComponent")]
    public class VoxelVolumeComponentNode : VoxelNodeMutable<VoxelVolumeComponent>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
