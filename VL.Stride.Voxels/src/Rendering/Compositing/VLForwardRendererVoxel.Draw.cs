using System.Runtime.CompilerServices;
using Stride.Core.Diagnostics;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using VL.Lib.Mathematics;
using Size2 = Stride.Core.Mathematics.Size2;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    public partial class VLForwardRendererVoxel
    {
        private static string TexInfo(Texture t) =>
            t == null
                ? "null"
                : $"{t.ViewWidth}x{t.ViewHeight} fmt={t.Format} msaa={t.MultisampleCount} flags={t.Flags}";

        private static bool HasRenderStageIndex(RenderView renderView, RenderStage renderStage)
        {
            if (renderView == null || renderStage == null || renderView.RenderStages == null)
                return false;

            foreach (var viewStage in renderView.RenderStages)
            {
                if (viewStage.Index == renderStage.Index)
                    return true;
            }

            return false;
        }

        protected virtual void DrawView(
            RenderContext context,
            RenderDrawContext drawContext,
            int eyeIndex,
            int eyeCount,
            bool renderPostFX = true
        )
        {
            var renderSystem = context.RenderSystem;

            var useVr = VRSettings != null && VRSettings.Enabled && VRSettings.VRDevice != null;
            if (useVr && !TryPrepareVRConstantBuffer(context, eyeIndex, eyeCount))
            {
                logger.Warning(
                    $"[DrawView] Skipping eye={eyeIndex}/{eyeCount} due to invalid VR constant buffer context."
                );
                return;
            }

            using (drawContext.PushRenderTargetsAndRestore())
            {
                if (OpaqueRenderStage == null)
                    return;

                if (!HasRenderStageIndex(context.RenderView, OpaqueRenderStage))
                {
                    logger.Warning(
                        $"[DrawView] Skipping OpaqueRenderStage for RenderViewIndex={context.RenderView?.Index ?? -1}; stage index {OpaqueRenderStage.Index} not present in RenderView.RenderStages."
                    );
                    return;
                }

                try
                {
                    renderSystem.Draw(drawContext, context.RenderView, OpaqueRenderStage);
                }
                catch (System.InvalidOperationException ex)
                {
                    logger.Warning(
                        $"[DrawView] renderSystem.Draw skipped for RenderViewIndex={context.RenderView?.Index ?? -1}, StageIndex={OpaqueRenderStage.Index}: {ex.Message}"
                    );
                    return;
                }

                var colorTargetIndex = OpaqueRenderStage.OutputValidator.Find(
                    typeof(ColorTargetSemantic)
                );
                if (colorTargetIndex == -1)
                    return;

                var renderTargets = currentRenderTargets;
                var depthStencil = currentDepthStencil;

                bool hasAnyMsaaTarget = false;
                for (int i = 0; i < currentRenderTargets.Count; i++)
                {
                    if (
                        currentRenderTargets[i] != null
                        && currentRenderTargets[i].MultisampleCount != MultisampleCount.None
                    )
                    {
                        hasAnyMsaaTarget = true;
                        break;
                    }
                }

                if (renderPostFX && hasAnyMsaaTarget)
                {
                    ResolveMSAA(drawContext);
                    renderTargets = currentRenderTargetsNonMSAA;
                    depthStencil = currentDepthStencilNonMSAA;
                }

                if (renderPostFX && PostEffects != null)
                {
                    PostEffects.Draw(
                        drawContext,
                        OpaqueRenderStage.OutputValidator,
                        renderTargets.Items,
                        depthStencil,
                        viewOutputTarget
                    );
                }
                else if (renderPostFX && hasAnyMsaaTarget)
                {
                    drawContext.CommandList.Copy(renderTargets[colorTargetIndex], viewOutputTarget);
                }

                if (EnableVoxelDraw && VoxelRenderer != null)
                {
                    using (context.SaveViewportAndRestore())
                        VoxelRenderer.Draw(drawContext, shadowMapRenderer);
                }
            }
        }

        protected override void DrawCore(RenderContext context, RenderDrawContext drawContext)
        {
            using var _ = Profiler.Begin(DrawCoreKey);

            var viewport = drawContext.CommandList.Viewport;
            var viewportSettings = ViewportSettings;
            var vrSettings = VRSettings;
            var hasValidVrViews =
                vrSettings?.RenderViews != null && vrSettings.RenderViews.Count() >= 2;

            if (viewportSettings?.ViewportRenderInfo != null)
                viewportSettings.ViewportRenderInfo.RenderTargetSize = viewport.Size;

            using (drawContext.PushRenderTargetsAndRestore())
            {
                shadowMapRenderer?.Draw(drawContext);

                if (vrSystem != null && vrSettings != null)
                    vrSystem.Visible = vrSettings.Enabled;

                if (
                    vrSettings != null
                    && vrSettings.Enabled
                    && vrSettings.VRDevice != null
                    && hasValidVrViews
                )
                {
                    ViewCount = 2;

                    PrepareRenderTargets(drawContext, vrSettings.VRDevice.ActualRenderFrameSize);

                    for (int i = 0; i < 2; i++)
                    {
                        ViewIndex = i;
                        drawContext.CommandList.SetRenderTargets(
                            currentDepthStencil,
                            currentRenderTargets.Count,
                            currentRenderTargets.Items
                        );

                        using (context.PushRenderViewAndRestore(vrSettings.RenderViews[i]))
                        {
                            Clear?.Draw(drawContext);
                            DrawView(context, drawContext, i, 2);
                        }
                    }
                }
                else if (viewportSettings?.Enabled == true && viewportSettings.Views?.Count > 0)
                {
                    PrepareRenderTargets(
                        drawContext,
                        new Size2((int)viewport.Width, (int)viewport.Height)
                    );

                    ViewCount = viewportSettings.Views.Count;
                    drawContext.CommandList.SetRenderTargets(
                        currentDepthStencil,
                        currentRenderTargets.Count,
                        currentRenderTargets.Items
                    );

                    for (int i = 0; i < ViewCount; i++)
                    {
                        ViewIndex = i;
                        var currentView = viewportSettings.Views[i];

                        using (context.PushRenderViewAndRestore(currentView.View))
                        using (context.SaveViewportAndRestore())
                        {
                            context.ViewportState = currentViewportState;
                            context.ViewportState.Viewport0 = Unsafe.As<ViewportF, Viewport>(
                                ref currentView.Viewport
                            );
                            drawContext.CommandList.SetViewport(context.ViewportState.Viewport0);

                            Clear?.Draw(drawContext);
                            DrawView(context, drawContext, i, ViewCount, renderPostFX: false);
                            currentView.Renderer?.Draw(drawContext);
                        }
                    }
                }
                else
                {
                    PrepareRenderTargets(
                        drawContext,
                        new Size2((int)viewport.Width, (int)viewport.Height)
                    );

                    ViewCount = 1;
                    ViewIndex = 0;

                    drawContext.CommandList.SetRenderTargets(
                        currentDepthStencil,
                        currentRenderTargets.Count,
                        currentRenderTargets.Items
                    );
                    Clear?.Draw(drawContext);
                    DrawView(context, drawContext, 0, 1);
                }
            }

            currentRenderTargets.Clear();
            currentRenderTargetsNonMSAA.Clear();
            currentDepthStencil = null;
            currentDepthStencilNonMSAA = null;
        }
    }
}
