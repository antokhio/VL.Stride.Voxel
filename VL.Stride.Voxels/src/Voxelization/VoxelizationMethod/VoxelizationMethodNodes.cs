using Stride.Graphics;
using Stride.Rendering.Voxels;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels.Voxelization.VoxelizationMethod
{
    /// <summary>
    /// Voxelization along a single configurable axis.
    /// </summary>
    [ProcessNode(Name = "VoxelizationMethodSingleAxis")]
    public class VoxelizationMethodSingleAxisNode : ProcessNodeBase<VoxelizationMethodSingleAxis>
    {
        private readonly Cachable<VoxelizationMethodSingleAxis.Axis> _voxelizationAxis;
        private readonly Cachable<MultisampleCount> _multisampleCount;

        public VoxelizationMethodSingleAxisNode()
        {
            _voxelizationAxis = new(
                this,
                (x, v) => x.VoxelizationAxis = v,
                VoxelizationMethodSingleAxis.Axis.Y
            );
            _multisampleCount = new(this, (x, v) => x.MultisampleCount = v, MultisampleCount.X8);
        }

        public void SetVoxelizationAxis(
            VoxelizationMethodSingleAxis.Axis voxelizationAxis = VoxelizationMethodSingleAxis.Axis.Y
        ) => _voxelizationAxis.SetValue(voxelizationAxis);

        public void SetMultisampleCount(MultisampleCount multisampleCount = MultisampleCount.X8) =>
            _multisampleCount.SetValue(multisampleCount);
    }

    /// <summary>
    /// Voxelization along the dominant axis with configurable multisampling.
    /// </summary>
    [ProcessNode(Name = "VoxelizationMethodDominantAxis")]
    public class VoxelizationMethodDominantAxisNode
        : ProcessNodeBase<VoxelizationMethodDominantAxis>
    {
        private readonly Cachable<MultisampleCount> _multisampleCount;

        public VoxelizationMethodDominantAxisNode()
        {
            _multisampleCount = new(this, (x, v) => x.MultisampleCount = v, MultisampleCount.X8);
        }

        public void SetMultisampleCount(MultisampleCount multisampleCount = MultisampleCount.X8) =>
            _multisampleCount.SetValue(multisampleCount);
    }

    /// <summary>
    /// Voxelization along all three axes with configurable multisampling.
    /// </summary>
    [ProcessNode(Name = "VoxelizationMethodTriAxis")]
    public class VoxelizationMethodTriAxisNode : ProcessNodeBase<VoxelizationMethodTriAxis>
    {
        private readonly Cachable<MultisampleCount> _multisampleCount;

        public VoxelizationMethodTriAxisNode()
        {
            _multisampleCount = new(this, (x, v) => x.MultisampleCount = v, MultisampleCount.X8);
        }

        public void SetMultisampleCount(MultisampleCount multisampleCount = MultisampleCount.X8) =>
            _multisampleCount.SetValue(multisampleCount);
    }
}
