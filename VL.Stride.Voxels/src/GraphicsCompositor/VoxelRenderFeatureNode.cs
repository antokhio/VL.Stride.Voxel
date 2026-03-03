using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.GraphicsCompositor
{
    /// <summary>
    /// Enables voxel rendering in the graphics compositor.
    /// </summary>
    [ProcessNode(Name = "VoxelRenderFeature")]
    public class VoxelRenderFeatureNode : ProcessNodeBase<VoxelRenderFeature>
    {
        protected override bool IsImmutable => false;
    }
}
