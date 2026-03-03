using SharpDX;
using Stride.Graphics;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    public partial class VLForwardRendererVoxel
    {
        private void LogTextureDesc(string tag, TextureDescription d)
        {
            logger.Error(
                $"{tag} | {d.Width}x{d.Height} fmt={d.Format} flags={d.Flags} msaa={d.MultisampleCount} mips={d.MipLevels} array={d.ArraySize}"
            );
        }

        private void LogSharpDx(string tag, SharpDXException ex)
        {
            logger.Error($"{tag} FAILED | {ex.ResultCode} | {ex.Message}");
        }
    }
}
