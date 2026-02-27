using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;

namespace VL.Stride.Voxels.Lights.LightTypes
{
    /// <summary>
    /// Debug visualization for voxel data.
    /// </summary>
    [ProcessNode(Name = "VoxelDebug")]
    public class VoxelDebugNode : VoxelNodeMutable<VoxelDebug>
    {
        // No Cachable fields, no Set methods — just wraps the type
    }
}
