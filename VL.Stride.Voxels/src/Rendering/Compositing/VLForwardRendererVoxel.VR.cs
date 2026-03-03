using System.Runtime.InteropServices;
using Stride.Core.Storage;
using Stride.Rendering;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    public partial class VLForwardRendererVoxel
    {
        [StructLayout(LayoutKind.Sequential)]
        private struct PerViewVR
        {
            public int EyeIndex;
            public int EyeCount;
        }

        private unsafe bool TryPrepareVRConstantBuffer(
            RenderContext context,
            int eyeIndex,
            int eyeCount
        )
        {
            if (context?.RenderView == null)
                return false;

            var renderView = context.RenderView;

            foreach (var renderFeature in context.RenderSystem.RenderFeatures)
            {
                if (renderFeature is not RootEffectRenderFeature rootFeature)
                    continue;

                var logicalKey = rootFeature.CreateViewLogicalGroup("GlobalVR");
                var viewFeature = renderView.Features[renderFeature.Index];

                foreach (var viewLayout in viewFeature.Layouts)
                {
                    var logicalGroup = viewLayout.GetLogicalGroup(logicalKey);
                    if (logicalGroup.Hash == ObjectId.Empty)
                        continue;

                    if (renderView.Index < 0 || renderView.Index >= viewLayout.Entries.Count())
                        return false;

                    var resourceGroup = viewLayout.Entries[renderView.Index].Resources;
                    var mappedCB = (PerViewVR*)(
                        resourceGroup.ConstantBuffer.Data + logicalGroup.ConstantBufferOffset
                    );
                    mappedCB->EyeIndex = eyeIndex;
                    mappedCB->EyeCount = eyeCount;
                }
            }

            return true;
        }
    }
}
