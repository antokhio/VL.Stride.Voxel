using Stride.Core.Mathematics;
using Stride.Core.Storage;
using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Rendering.Images;

namespace VL.Stride.Voxels.Rendering.Compositing
{
    public partial class VLForwardRendererVoxel
    {
        protected static PixelFormat ComputeNonMSAADepthFormat(PixelFormat format)
        {
            switch (format)
            {
                case PixelFormat.R16_Float:
                case PixelFormat.R16_Typeless:
                case PixelFormat.D16_UNorm:
                    return PixelFormat.R16_Float;

                case PixelFormat.R32_Float:
                case PixelFormat.R32_Typeless:
                case PixelFormat.D32_Float:
                    return PixelFormat.R32_Float;

                // stencil info is lost in MSAA -> non-MSAA conversion path
                case PixelFormat.R24G8_Typeless:
                case PixelFormat.D24_UNorm_S8_UInt:
                case PixelFormat.R24_UNorm_X8_Typeless:
                    return PixelFormat.R32_Float;

                case PixelFormat.R32G8X24_Typeless:
                case PixelFormat.D32_Float_S8X24_UInt:
                case PixelFormat.R32_Float_X8X24_Typeless:
                    return PixelFormat.R32_Float;

                default:
                    throw new NotSupportedException($"Unsupported depth format [{format}]");
            }
        }

        /// <summary>
        /// Resolves MSAA render/depth into non-MSAA textures used by post effects.
        /// </summary>
        private void ResolveMSAA(RenderDrawContext drawContext)
        {
            currentRenderTargetsNonMSAA.Resize(currentRenderTargets.Count, false);

            for (int i = 0; i < currentRenderTargets.Count; i++)
            {
                var input = currentRenderTargets[i];
                if (input == null)
                    continue;

                // If this RT is not MSAA, just use it directly (no resolve).
                if (input.MultisampleCount == MultisampleCount.None)
                {
                    currentRenderTargetsNonMSAA[i] = input;
                    continue;
                }

                var outputDescription = TextureDescription.New2D(
                    input.ViewWidth,
                    input.ViewHeight,
                    1,
                    input.Format,
                    TextureFlags.ShaderResource | TextureFlags.RenderTarget
                );

                var output = PushScopedResource(
                    drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(outputDescription)
                );
                currentRenderTargetsNonMSAA[i] = output;

                MSAAResolver.Resolve(drawContext, input, output);
            }

            // Depth
            if (
                currentDepthStencil != null
                && currentDepthStencil.MultisampleCount != MultisampleCount.None
            )
            {
                currentDepthStencilNonMSAA = viewDepthStencil;
                MSAAResolver.Resolve(drawContext, currentDepthStencil, currentDepthStencilNonMSAA);
            }
            else
            {
                currentDepthStencilNonMSAA = currentDepthStencil;
            }
        }

        private void CopyOrScaleTexture(
            RenderDrawContext drawContext,
            Texture input,
            Texture output,
            ImageScaler mirrorScaler
        )
        {
            if (input.Size != output.Size)
            {
                mirrorScaler.SetInput(0, input);
                mirrorScaler.SetOutput(output);
                mirrorScaler.Draw(drawContext);
            }
            else
            {
                drawContext.CommandList.Copy(input, output);
            }
        }

        private Texture ResolveDepthAsSRV(RenderDrawContext context)
        {
            if (!BindDepthAsResourceDuringTransparentRendering)
                return null;

            // Guard unsupported path: MSAA depth SRV not available on this GPU/platform.
            if (
                actualMultisampleCount != MultisampleCount.None
                && !GraphicsDevice.Features.HasMultisampleDepthAsSRV
            )
                return null;

            var depthStencil = context.CommandList.DepthStencilBuffer;
            var depthStencilSRV = context.Resolver.ResolveDepthStencil(depthStencil);

            var renderView = context.RenderContext.RenderView;

            foreach (var renderFeature in context.RenderContext.RenderSystem.RenderFeatures)
            {
                if (!(renderFeature is RootEffectRenderFeature rootFeature))
                    continue;

                var depthLogicalKey = rootFeature.CreateViewLogicalGroup("Depth");
                var viewFeature = renderView.Features[renderFeature.Index];

                foreach (var viewLayout in viewFeature.Layouts)
                {
                    var depthLogicalGroup = viewLayout.GetLogicalGroup(depthLogicalKey);
                    if (depthLogicalGroup.Hash == ObjectId.Empty)
                        continue;

                    var resourceGroup = viewLayout.Entries[renderView.Index].Resources;
                    resourceGroup.DescriptorSet.SetShaderResourceView(
                        depthLogicalGroup.DescriptorSlotStart,
                        depthStencilSRV
                    );
                }
            }

            // Rebind RTs with read-only depth view
            context.CommandList.SetRenderTargets(
                null,
                context.CommandList.RenderTargetCount,
                context.CommandList.RenderTargets
            );

            var depthStencilRO = context.Resolver.GetDepthStencilAsRenderTarget(
                depthStencil,
                depthStencilROCached
            );
            if (depthStencilRO != depthStencilROCached)
            {
                depthStencilROCached?.Dispose();
                depthStencilROCached = depthStencilRO;
            }

            context.CommandList.SetRenderTargets(
                depthStencilROCached,
                context.CommandList.RenderTargetCount,
                context.CommandList.RenderTargets
            );

            return depthStencilSRV;
        }

        private Texture ResolveRenderTargetAsSRV(RenderDrawContext drawContext)
        {
            if (!BindOpaqueAsResourceDuringTransparentRendering)
                return null;

            var renderTarget = drawContext.CommandList.RenderTargets[0];
            var renderTargetTexture = Context.Allocator.GetTemporaryTexture2D(
                renderTarget.Description
            );

            drawContext.CommandList.Copy(renderTarget, renderTargetTexture);

            var renderView = drawContext.RenderContext.RenderView;
            foreach (var renderFeature in drawContext.RenderContext.RenderSystem.RenderFeatures)
            {
                if (!(renderFeature is RootEffectRenderFeature rootFeature))
                    continue;

                var opaqueLogicalKey = rootFeature.CreateViewLogicalGroup("Opaque");
                var viewFeature = renderView.Features[renderFeature.Index];

                foreach (var viewLayout in viewFeature.Layouts)
                {
                    var opaqueLogicalGroup = viewLayout.GetLogicalGroup(opaqueLogicalKey);
                    if (opaqueLogicalGroup.Hash == ObjectId.Empty)
                        continue;

                    var resourceGroup = viewLayout.Entries[renderView.Index].Resources;
                    resourceGroup.DescriptorSet.SetShaderResourceView(
                        opaqueLogicalGroup.DescriptorSlotStart,
                        renderTargetTexture
                    );
                }
            }

            return renderTargetTexture;
        }

        private void PrepareRenderTargets(
            RenderDrawContext drawContext,
            Texture outputRenderTarget,
            Texture outputDepthStencil
        )
        {
            if (OpaqueRenderStage == null)
                return;

            var renderTargets = OpaqueRenderStage.OutputValidator.RenderTargets;
            currentRenderTargets.Resize(renderTargets.Count, false);

            for (int index = 0; index < renderTargets.Count; index++)
            {
                if (
                    renderTargets[index].Semantic is ColorTargetSemantic
                    && PostEffects == null
                    && actualMultisampleCount == MultisampleCount.None
                )
                {
                    currentRenderTargets[index] = outputRenderTarget;
                }
                else
                {
                    var description = renderTargets[index];

                    var flags = TextureFlags.RenderTarget;
                    if (actualMultisampleCount == MultisampleCount.None)
                        flags |= TextureFlags.ShaderResource;

                    var arraySize = Math.Max(1, outputRenderTarget.ArraySize);

                    var textureDescription = TextureDescription.New2D(
                        outputRenderTarget.Width,
                        outputRenderTarget.Height,
                        outputRenderTarget.MipLevels > 0 ? outputRenderTarget.MipLevels : 1,
                        description.Format,
                        flags,
                        arraySize,
                        GraphicsResourceUsage.Default,
                        actualMultisampleCount
                    );

                    try
                    {
                        currentRenderTargets[index] = PushScopedResource(
                            drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(
                                textureDescription
                            )
                        );
                    }
                    catch (SharpDX.SharpDXException)
                    {
                        // Fallback: some drivers reject SRGB/format combos or SRV-capable RT creation here.
                        textureDescription.Flags = TextureFlags.RenderTarget;
                        textureDescription.MultisampleCount = MultisampleCount.None;
                        textureDescription.ArraySize = 1;
                        textureDescription.MipLevels = 1;

                        currentRenderTargets[index] = PushScopedResource(
                            drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(
                                textureDescription
                            )
                        );
                    }
                }

                drawContext.CommandList.ResourceBarrierTransition(
                    currentRenderTargets[index],
                    GraphicsResourceState.RenderTarget
                );
            }

            // Prepare depth
            if (actualMultisampleCount == MultisampleCount.None)
            {
                currentDepthStencil = outputDepthStencil;
            }
            else
            {
                var d = outputDepthStencil.Description;

                // Start with safest MSAA depth: DepthStencil only, no SRV
                var depthDesc = TextureDescription.New2D(
                    d.Width,
                    d.Height,
                    1,
                    d.Format,
                    TextureFlags.DepthStencil,
                    1,
                    GraphicsResourceUsage.Default,
                    actualMultisampleCount
                );

                try
                {
                    currentDepthStencil = PushScopedResource(
                        drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(depthDesc)
                    );
                }
                catch (SharpDX.SharpDXException)
                {
                    // Fallback 1: disable MSAA for depth
                    depthDesc.MultisampleCount = MultisampleCount.None;

                    try
                    {
                        currentDepthStencil = PushScopedResource(
                            drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(depthDesc)
                        );
                    }
                    catch (SharpDX.SharpDXException)
                    {
                        // Fallback 2: force canonical depth format
                        depthDesc.Format = DepthBufferFormat; // D24_UNorm_S8_UInt
                        depthDesc.MultisampleCount = MultisampleCount.None;
                        depthDesc.ArraySize = 1;
                        depthDesc.MipLevels = 1;

                        currentDepthStencil = PushScopedResource(
                            drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(depthDesc)
                        );
                    }
                }
            }

            drawContext.CommandList.ResourceBarrierTransition(
                currentDepthStencil,
                GraphicsResourceState.DepthWrite
            );
        }

        /// <summary>
        /// Prepares view output/depth targets then builds stage render targets for the frame.
        /// </summary>
        protected virtual void PrepareRenderTargets(
            RenderDrawContext drawContext,
            Size2 renderTargetsSize
        )
        {
            viewOutputTarget = drawContext.CommandList.RenderTarget;
            if (drawContext.CommandList.RenderTargetCount == 0)
                viewOutputTarget = null;

            viewDepthStencil = drawContext.CommandList.DepthStencilBuffer;

            // Create output if needed
            if (
                viewOutputTarget == null
                || viewOutputTarget.MultisampleCount != MultisampleCount.None
            )
            {
                viewOutputTarget = PushScopedResource(
                    drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(
                        TextureDescription.New2D(
                            renderTargetsSize.Width,
                            renderTargetsSize.Height,
                            1,
                            PixelFormat.R8G8B8A8_UNorm_SRgb,
                            TextureFlags.ShaderResource | TextureFlags.RenderTarget
                        )
                    )
                );
            }

            // Create depth if needed
            if (
                viewDepthStencil == null
                || viewDepthStencil.MultisampleCount != MultisampleCount.None
            )
            {
                viewDepthStencil = PushScopedResource(
                    drawContext.GraphicsContext.Allocator.GetTemporaryTexture2D(
                        TextureDescription.New2D(
                            renderTargetsSize.Width,
                            renderTargetsSize.Height,
                            1,
                            DepthBufferFormat,
                            TextureFlags.ShaderResource | TextureFlags.DepthStencil
                        )
                    )
                );
            }

            PrepareRenderTargets(drawContext, viewOutputTarget, viewDepthStencil);
        }

        protected override void Destroy()
        {
            PostEffects?.Dispose();
            depthStencilROCached?.Dispose();
        }

        // ---- Placeholders (replace with original VL implementations if needed) ----

        private void PrepareLightprobeConstantBuffer(RenderContext context)
        {
            // TODO: copy implementation from your VLForwardRenderer baseline if required
        }

        private void BakeLightProbes(RenderContext context, RenderDrawContext drawContext)
        {
            // TODO: copy implementation from your VLForwardRenderer baseline if required
        }

        private unsafe void ComputeCommonViewMatrices(
            RenderContext context,
            Matrix* viewMatrices,
            Matrix* projectionMatrices
        )
        {
            // TODO: copy implementation from your VLForwardRenderer baseline if required
        }
    }
}
