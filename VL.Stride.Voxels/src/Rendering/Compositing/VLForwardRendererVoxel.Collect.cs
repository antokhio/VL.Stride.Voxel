using Stride.Core;
using Stride.Core.Diagnostics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    public partial class VLForwardRendererVoxel
    {
        protected virtual void CollectStages(RenderContext context)
        {
            if (OpaqueRenderStage != null)
            {
                OpaqueRenderStage.OutputValidator.BeginCustomValidation(
                    context.RenderOutput.DepthStencilFormat,
                    context.RenderOutput.MultisampleCount
                );
                ValidateOpaqueStageOutput(OpaqueRenderStage.OutputValidator, context);
                OpaqueRenderStage.OutputValidator.EndCustomValidation();
            }

            TransparentRenderStage?.OutputValidator.Validate(ref context.RenderOutput);

            if (GBufferRenderStage != null && LightProbes)
            {
                GBufferRenderStage.Output = new RenderOutputDescription(
                    PixelFormat.None,
                    context.RenderOutput.DepthStencilFormat
                );
            }
        }

        protected virtual void ValidateOpaqueStageOutput(
            RenderOutputValidator renderOutputValidator,
            RenderContext renderContext
        )
        {
            // Temporary safety: only RT0, avoid null ShadingColor1 permutations.
            renderOutputValidator.Add<ColorTargetSemantic>(
                renderContext.RenderOutput.RenderTargetFormat0
            );

            if (PostEffects == null)
                return;

            if (PostEffects.RequiresNormalBuffer)
            {
                renderOutputValidator.Add<ColorTargetSemantic>(
                    Platform.Type == PlatformType.Android || Platform.Type == PlatformType.iOS
                        ? PixelFormat.R16G16B16A16_Float
                        : PixelFormat.R10G10B10A2_UNorm
                );
            }

            if (PostEffects.RequiresSpecularRoughnessBuffer)
                renderOutputValidator.Add<ColorTargetSemantic>(PixelFormat.R8G8B8A8_UNorm);

            if (PostEffects.RequiresVelocityBuffer)
                renderOutputValidator.Add<ColorTargetSemantic>(PixelFormat.R16G16_Float);

            if (SubsurfaceScatteringBlurEffect != null)
                renderOutputValidator.Add<ColorTargetSemantic>(PixelFormat.R16_Float);
        }

        protected virtual void CollectView(
            RenderContext context,
            int eyeIndex,
            int eyeCount,
            bool useVr
        )
        {
            if (useVr && !TryPrepareVRConstantBuffer(context, eyeIndex, eyeCount))
            {
                logger.Warning(
                    $"[CollectView] Skipping eye={eyeIndex}/{eyeCount} due to invalid VR constant buffer context."
                );
                return;
            }

            // Critical: register stages on the active RenderView
            if (OpaqueRenderStage != null)
                context.RenderView.RenderStages.Add(OpaqueRenderStage);

            if (TransparentRenderStage != null)
                context.RenderView.RenderStages.Add(TransparentRenderStage);

            if (GBufferRenderStage != null && LightProbes)
                context.RenderView.RenderStages.Add(GBufferRenderStage);

            LightShafts?.Collect(context);
            PostEffects?.Collect(context);
        }

        protected override void CollectCore(RenderContext context)
        {
            using var _ = Profiler.Begin(CollectCoreKey);

            if (!Enabled)
                return;

            if (EnableVoxelCollect && VoxelRenderer != null)
                VoxelRenderer.Collect(Context, shadowMapRenderer);

            if (context.RenderView == null)
                return;

            using (context.SaveRenderOutputAndRestore())
            {
                shadowMapRenderer?.RenderViewsWithShadows.Add(context.RenderView);

                context.RenderOutput = new RenderOutputDescription(
                    PostEffects != null
                        ? PixelFormat.R16G16B16A16_Float
                        : context.RenderOutput.RenderTargetFormat0,
                    DepthBufferFormat,
                    MSAALevel
                );

                CollectStages(context);

                var viewportSettings = ViewportSettings;
                var vrSettings = VRSettings;
                var hasValidVrViews =
                    vrSettings?.RenderViews != null && vrSettings.RenderViews.Count() >= 2;
                var useVr =
                    vrSettings != null
                    && vrSettings.Enabled
                    && vrSettings.VRDevice != null
                    && hasValidVrViews;

                if (vrSystem != null && vrSettings != null)
                    vrSystem.Visible = vrSettings.Enabled;

                if (useVr)
                {
                    ViewCount = 2;
                    for (int i = 0; i < 2; i++)
                    {
                        ViewIndex = i;
                        using (context.PushRenderViewAndRestore(vrSettings.RenderViews[i]))
                        {
                            context.RenderSystem.Views.Add(context.RenderView);
                            CollectView(context, i, 2, useVr: true);
                        }
                    }
                }
                else if (viewportSettings?.Enabled == true && viewportSettings.Views?.Count > 0)
                {
                    ViewCount = viewportSettings.Views.Count;
                    for (int i = 0; i < ViewCount; i++)
                    {
                        ViewIndex = i;
                        using (context.PushRenderViewAndRestore(viewportSettings.Views[i].View))
                        {
                            context.RenderSystem.Views.Add(context.RenderView);
                            CollectView(context, i, ViewCount, useVr: false);
                        }
                    }
                }
                else
                {
                    ViewCount = 1;
                    ViewIndex = 0;
                    CollectView(context, 0, 1, useVr: false);
                }
            }
        }
    }
}
