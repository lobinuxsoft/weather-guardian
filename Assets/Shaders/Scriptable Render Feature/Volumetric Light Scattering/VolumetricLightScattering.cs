using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumetricLightScattering : ScriptableRendererFeature
{
    class LightScatteringPass : ScriptableRenderPass
    {
        private readonly ProfilingSampler vlProfilingSampler = new ProfilingSampler("VolumetricLightScattering");
        private readonly RTHandle occluders;
        private readonly float resolutionScale;
        private readonly float intensity;
        private readonly float blurWidth;
        private readonly Material occludersMaterial;
        private readonly Material radialBlurMaterial;
        private readonly List<ShaderTagId> shaderTagIdList = new List<ShaderTagId>();

        private FilteringSettings filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        private RTHandle destinationColor;

        public LightScatteringPass(VolumetricLightScatteringSettings settings)
        {
            occluders = RTHandles.Alloc("_OccludersMap", name: "_OccludersMap");
            occludersMaterial = new Material(Shader.Find("Hidden/RW/UnlitColor"));
            radialBlurMaterial = new Material(Shader.Find("Hidden/RW/RadialBlur"));
            resolutionScale = settings.resolutionScale;
            intensity = settings.intensity;
            blurWidth = settings.blurWidth;

            shaderTagIdList.Add(new ShaderTagId("UniversalForward"));
            shaderTagIdList.Add(new ShaderTagId("UniversalForwardOnly"));
            shaderTagIdList.Add(new ShaderTagId("LightweightForward"));
            shaderTagIdList.Add(new ShaderTagId("SRPDefaultUnlit"));
        }

        public void Dispose()
        {
            occluders?.Release();
            destinationColor?.Release();
        }

        public void SetUp(RTHandle destinationColor)
        {
            this.destinationColor = destinationColor;
        }

        // This method is called before executing the render pass.
        // It can be used to configure render targets and their clear state. Also to create temporary render target textures.
        // When empty this render pass will render to the active camera render target.
        // You should never call CommandBuffer.SetRenderTarget. Instead call <c>ConfigureTarget</c> and <c>ConfigureClear</c>.
        // The render pipeline will ensure target setup and clearing happens in a performant manner.
        public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
        {
            RenderTextureDescriptor cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cameraTextureDescriptor.depthBufferBits = 0;
            cameraTextureDescriptor.width = Mathf.RoundToInt(cameraTextureDescriptor.width * resolutionScale);
            cameraTextureDescriptor.height = Mathf.RoundToInt(cameraTextureDescriptor.height * resolutionScale);
            cmd.GetTemporaryRT(Shader.PropertyToID(occluders.name), cameraTextureDescriptor, FilterMode.Bilinear);
            ConfigureTarget(occluders);
            ConfigureClear(ClearFlag.All, Color.black);
        }

        // Here you can implement the rendering logic.
        // Use <c>ScriptableRenderContext</c> to issue drawing commands or execute command buffers
        // https://docs.unity3d.com/ScriptReference/Rendering.ScriptableRenderContext.html
        // You don't have to call ScriptableRenderContext.submit, the render pipeline will call it at specific points in the pipeline.
        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            Camera camera = renderingData.cameraData.camera;

            if (!occludersMaterial || !radialBlurMaterial) return;

            CommandBuffer cmd = CommandBufferPool.Get();

            using (new ProfilingScope(cmd, vlProfilingSampler))
            {
                context.ExecuteCommandBuffer(cmd);
                cmd.Clear();

                context.DrawSkybox(camera);

                DrawingSettings drawSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, SortingCriteria.CommonOpaque);
                drawSettings.overrideMaterial = occludersMaterial;

                context.DrawRenderers(renderingData.cullResults, ref drawSettings, ref filteringSettings);

                // You get a reference to the sun from RenderSettings.
                // You need the forward vector of the sun because directional lights don’t have a position in space.
                Vector3 sunDirectionWorldSpace = RenderSettings.sun.transform.forward;

                // Get the camera position.
                Vector3 cameraPositionWorldSpace = camera.transform.position;

                // This gives you a unit vector that goes from the camera towards the sun.
                // You’ll use this for the sun’s position.
                Vector3 sunPositionWorldSpace = cameraPositionWorldSpace + sunDirectionWorldSpace;

                // The shader expects a viewport space position, but you did your calculations in world space.
                // To fix this, you use WorldToViewportPoint() to transform the point-to-camera viewport space.
                Vector3 sunPositionViewportSpace = camera.WorldToViewportPoint(sunPositionWorldSpace);

                Color sunColor = RenderSettings.sun.useColorTemperature ?
                    Mathf.CorrelatedColorTemperatureToRGB(RenderSettings.sun.colorTemperature) :
                    RenderSettings.sun.color;

                radialBlurMaterial.SetVector("_Center", new Vector4(sunPositionViewportSpace.x, sunPositionViewportSpace.y, 0, 0));
                radialBlurMaterial.SetFloat("_Intensity", intensity);
                radialBlurMaterial.SetFloat("_BlurWidth", blurWidth);
                radialBlurMaterial.SetColor("_LightColor", sunColor);

                cmd.Blit(occluders.nameID, destinationColor.nameID, radialBlurMaterial, 0);
            }

            context.ExecuteCommandBuffer(cmd);
            cmd.Clear();
            CommandBufferPool.Release(cmd);
        }

        // Cleanup any allocated resources that were created during the execution of this render pass.
        public override void OnCameraCleanup(CommandBuffer cmd)
        {
            destinationColor = null;
            cmd.ReleaseTemporaryRT(Shader.PropertyToID(occluders.name));
        }
    }

    LightScatteringPass lightScatteringPass;

    public VolumetricLightScatteringSettings settings = new VolumetricLightScatteringSettings();

    /// <inheritdoc/>
    public override void Create()
    {
        lightScatteringPass = new LightScatteringPass(settings);

        // Configures where the render pass should be injected.
        lightScatteringPass.renderPassEvent = RenderPassEvent.BeforeRenderingPostProcessing;
    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        renderer.EnqueuePass(lightScatteringPass);
    }

    public override void SetupRenderPasses(ScriptableRenderer renderer, in RenderingData renderingData)
    {
        lightScatteringPass.SetUp(renderer.cameraColorTargetHandle);
    }

    protected override void Dispose(bool disposing)
    {
        lightScatteringPass.Dispose();
    }
}

[System.Serializable]
public class VolumetricLightScatteringSettings
{
    [Header("Properties")]
    [Range(0.1f, 1f)]
    public float resolutionScale = 0.5f;

    [Range(0.0f, 1.0f)]
    public float intensity = 1.0f;

    [Range(0.0f, 1.0f)]
    public float blurWidth = 0.85f;
}