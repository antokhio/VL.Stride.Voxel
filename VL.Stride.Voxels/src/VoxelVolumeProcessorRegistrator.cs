using Microsoft.Extensions.DependencyInjection;
using Stride.Engine;
using Stride.Engine.Processors;
using VL.Core;
using VL.Core.Import;

namespace VL.Stride.Rendering.Voxels
{
    [ProcessNode]
    public class VoxelVolumeProcessorRegistrator
    {
        public VoxelVolumeProcessorRegistrator(AppHost appHost)
        {
            var game = appHost.Services.GetRequiredService<Game>();
            var voxelVolumeProcessor =
                game.SceneSystem.SceneInstance.GetProcessor<VoxelVolumeProcessor>();

            if (voxelVolumeProcessor != null) { }
        }
    }
}
