using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Voxel volume component for configuring voxelization.
    /// </summary>
    [ProcessNode(Name = "VoxelVolumeComponent")]
    public class VoxelVolumeComponentNode : VoxelNodeImmutable<VoxelVolumeComponent> { }
}
