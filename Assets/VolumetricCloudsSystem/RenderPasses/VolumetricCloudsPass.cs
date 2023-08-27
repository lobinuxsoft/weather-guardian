using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
using UnityEngine;

public class VolumetricCloudsPass : ScriptableRenderPass
{
    public FilterMode filterMode { get; set; }
    public VolumetricCloudsFeature.Settings settings;

    private RenderTargetIdentifier source;
    private RenderTargetIdentifier destination;
    private int temporaryRTId = Shader.PropertyToID("_TempRT");

    private int sourceId;
    private int destinationId;
    private bool isSourceAndDestinationSameTarget;

    private string profilerTag;

    public VolumetricCloudsPass(string tag) => profilerTag = tag;

    public override void OnCameraSetup(CommandBuffer cmd, ref RenderingData renderingData)
    {
        RenderTextureDescriptor blitTargetDescriptor = renderingData.cameraData.cameraTargetDescriptor;
        blitTargetDescriptor.depthBufferBits = 0;

        isSourceAndDestinationSameTarget = settings.sourceType == settings.destinationType &&
            (settings.sourceType == BufferType.CameraColor || settings.sourceTextureId == settings.destinationTextureId);

        var renderer = renderingData.cameraData.renderer;

        if (settings.sourceType == BufferType.CameraColor)
        {
            sourceId = -1;
            source = renderer.cameraColorTargetHandle;
        }
        else
        {
            sourceId = Shader.PropertyToID(settings.sourceTextureId);
            cmd.GetTemporaryRT(sourceId, blitTargetDescriptor, filterMode);
            source = new RenderTargetIdentifier(sourceId);
        }

        if (isSourceAndDestinationSameTarget)
        {
            destinationId = temporaryRTId;
            cmd.GetTemporaryRT(destinationId, blitTargetDescriptor, filterMode);
            destination = new RenderTargetIdentifier(destinationId);
        }
        else if (settings.destinationType == BufferType.CameraColor)
        {
            destinationId = -1;
            destination = renderer.cameraColorTargetHandle;
        }
        else
        {
            destinationId = Shader.PropertyToID(settings.destinationTextureId);
            cmd.GetTemporaryRT(destinationId, blitTargetDescriptor, filterMode);
            destination = new RenderTargetIdentifier(destinationId);
        }
    }

    /// <inheritdoc/>
    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        CommandBuffer cmd = CommandBufferPool.Get(profilerTag);

        // Can't read and write to same color target, create a temp render target to blit. 
        if (isSourceAndDestinationSameTarget)
        {
            cmd.Blit(source, destination, settings.blitMaterial, settings.blitMaterialPassIndex);
            cmd.Blit(destination, source);
        }
        else
        {
            cmd.Blit(source, destination, settings.blitMaterial, settings.blitMaterialPassIndex);
        }

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }

    public override void OnCameraCleanup(CommandBuffer cmd)
    {
        if (destinationId != -1)
            cmd.ReleaseTemporaryRT(destinationId);

        if (source == destination && sourceId != -1)
            cmd.ReleaseTemporaryRT(sourceId);
    }
}