using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    /// <summary>
    /// Enables voxel rendering in the graphics compositor.
    /// </summary>
    [ProcessNode(Name = "VoxelRenderFeature")]
    public class VoxelRenderFeatureNode : VoxelNodeMutable<VoxelRenderFeature> { }
}
