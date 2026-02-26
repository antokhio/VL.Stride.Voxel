using Stride.Graphics;
using Stride.Rendering;
using Stride.Rendering.Compositing;
using Stride.Rendering.Images;
using Stride.Rendering.SubsurfaceScattering;
using Stride.Rendering.Voxels;
using Stride.Rendering.Voxels.Debug;
using VL.Core.Import;
using VL.Stride.Rendering;
using VL.Stride.VoxelGI.Helpers;

namespace VL.Stride.VoxelGI.Rendering.Compositing
{
    [ProcessNode(Name = "VoxelGIForwardRederer")]
    public class VoxelGIForwardRederer : VoxelGINodeBase<VLVoxelGIForwardRenderer>
    {
        private static readonly MSAAResolver _defaultResolver = new();

        private readonly Cachable<ClearRenderer> _clear;
        private readonly Cachable<RenderStage> _opaqueRenderStage;
        private readonly Cachable<RenderStage> _transparentRenderStage;
        private readonly CachableList<RenderStage> _shadowMapRenderStages;
        private readonly Cachable<RenderStage> _gBufferRenderStage;
        private readonly Cachable<IPostProcessingEffects> _postEffects;
        private readonly Cachable<LightShafts> _lightShafts;
        private readonly Cachable<VRRendererSettings> _vrSettings;
        private readonly Cachable<ViewportSettings> _viewportSettings;
        private readonly Cachable<SubsurfaceScatteringBlur> _subsurfaceScatteringBlurEffect;
        private readonly Cachable<MultisampleCount> _msaaLevel;
        private readonly Cachable<MSAAResolver> _msaaResolver;
        private readonly Cachable<bool> _bindDepthAsResourceDuringTransparentRendering;
        private readonly Cachable<bool> _bindOpaqueAsResourceDuringTransparentRendering;
        private readonly Cachable<IVoxelRenderer> _voxelRenderer;
        private readonly Cachable<VoxelDebug> _voxelVisualization;

        private readonly Cachable<bool> _enabled;

        public void SetClear(ClearRenderer clear) => _clear.SetValue(clear);

        public void SetOpaqueRenderStage(RenderStage opaqueRenderStage) =>
            _opaqueRenderStage.SetValue(opaqueRenderStage);

        public void SetTransparentRenderStage(RenderStage transparentRenderStage) =>
            _transparentRenderStage.SetValue(transparentRenderStage);

        public void SetShadowMapRenderStages(IReadOnlyList<RenderStage> shadowMapRenderStages) =>
            _shadowMapRenderStages.SetValue(shadowMapRenderStages);

        public void SetGBufferRenderStage(RenderStage gBufferRenderStage) =>
            _gBufferRenderStage.SetValue(gBufferRenderStage);

        public void SetPostEffects(IPostProcessingEffects postEffects) =>
            _postEffects.SetValue(postEffects);

        public void SetLightShafts(LightShafts lightShafts) => _lightShafts.SetValue(lightShafts);

        public void SetVRSettings(VRRendererSettings vrSettings) =>
            _vrSettings.SetValue(vrSettings);

        public void SetViewportSettings(ViewportSettings viewportSettings) =>
            _viewportSettings.SetValue(viewportSettings);

        public void SetSubsurfaceScatteringBlurEffect(
            SubsurfaceScatteringBlur subsurfaceScatteringBlurEffect
        ) => _subsurfaceScatteringBlurEffect.SetValue(subsurfaceScatteringBlurEffect);

        public void SetMSAALevel(MultisampleCount msaaLevel) => _msaaLevel.SetValue(msaaLevel);

        public void SetMSAAResolver(MSAAResolver msaaResolver) =>
            _msaaResolver.SetValue(msaaResolver);

        public void SetBindDepthAsResourceDuringTransparentRendering(
            bool bindDepthAsResourceDuringTransparentRendering
        ) =>
            _bindDepthAsResourceDuringTransparentRendering.SetValue(
                bindDepthAsResourceDuringTransparentRendering
            );

        public void SetBindOpaqueAsResourceDuringTransparentRendering(
            bool bindOpaqueAsResourceDuringTransparentRendering
        ) =>
            _bindOpaqueAsResourceDuringTransparentRendering.SetValue(
                bindOpaqueAsResourceDuringTransparentRendering
            );

        public void SetVoxelRenderer(IVoxelRenderer voxelRenderer) =>
            _voxelRenderer.SetValue(voxelRenderer);

        public void SetVoxelVisualization(VoxelDebug voxelVisualization) =>
            _voxelVisualization.SetValue(voxelVisualization);

        public void SetEnabled(bool enabled) => _enabled.SetValue(enabled);

        public VoxelGIForwardRederer()
        {
            _clear = new(this, x => x.Clear, (x, v) => x.Clear = v, null);
            _opaqueRenderStage = new(
                this,
                x => x.OpaqueRenderStage,
                (x, v) => x.OpaqueRenderStage = v
            );
            _transparentRenderStage = new(
                this,
                x => x.TransparentRenderStage,
                (x, v) => x.TransparentRenderStage = v
            );
            _shadowMapRenderStages = new(this, x => x.ShadowMapRenderStages);
            _gBufferRenderStage = new(
                this,
                x => x.GBufferRenderStage,
                (x, v) => x.GBufferRenderStage = v
            );
            _postEffects = new(this, x => x.PostEffects, (x, v) => x.PostEffects = v);
            _lightShafts = new(this, x => x.LightShafts, (x, v) => x.LightShafts = v);
            _vrSettings = new(this, x => x.VRSettings, (x, v) => x.VRSettings = v);
            _viewportSettings = new(
                this,
                x => x.ViewportSettings,
                (x, v) => x.ViewportSettings = v
            );
            _subsurfaceScatteringBlurEffect = new(
                this,
                x => x.SubsurfaceScatteringBlurEffect,
                (x, v) => x.SubsurfaceScatteringBlurEffect = v
            );
            _msaaLevel = new(this, x => x.MSAALevel, (x, v) => x.MSAALevel = v);
            _msaaResolver = new(
                this,
                x => x.MSAAResolver,
                (x, v) =>
                {
                    var s = x.MSAAResolver;
                    var y = v ?? _defaultResolver;
                    s.FilterType = y.FilterType;
                    s.FilterRadius = y.FilterRadius;
                }
            );
            _bindDepthAsResourceDuringTransparentRendering = new(
                this,
                x => x.BindDepthAsResourceDuringTransparentRendering,
                (x, v) => x.BindDepthAsResourceDuringTransparentRendering = v
            );
            _bindOpaqueAsResourceDuringTransparentRendering = new(
                this,
                x => x.BindOpaqueAsResourceDuringTransparentRendering,
                (x, v) => x.BindOpaqueAsResourceDuringTransparentRendering = v
            );
            _voxelRenderer = new(this, x => x.VoxelRenderer, (x, v) => x.VoxelRenderer = v);
            _voxelVisualization = new(
                this,
                x => x.VoxelVisualization,
                (x, v) => x.VoxelVisualization = v
            );

            _enabled = new(this, x => x.Enabled, (x, v) => x.Enabled = v, true);
        }
    }
}
