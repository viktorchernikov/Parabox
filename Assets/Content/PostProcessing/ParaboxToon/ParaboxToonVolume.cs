using System;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine;
using UnityEditor;

[Serializable, VolumeComponentMenu("Post-processing/ParaboxToonVolume")]
public sealed class ParaboxToonVolume : CustomPostProcessVolumeComponent, IPostProcessComponent
{
    [Tooltip("Controls the intensity of the effect.")]
    public ClampedFloatParameter intensity = new ClampedFloatParameter(0f, 0f, 1f);
    public ClampedFloatParameter outlineNormalThreshold = new ClampedFloatParameter(0.5f, 0.001f, 10f);
    public ClampedFloatParameter ditherSize = new ClampedFloatParameter(0.1f, 0.001f, 10f);
    public ClampedFloatParameter ditherSpread = new ClampedFloatParameter(0.1f, 0.001f, 10f);
    public ClampedIntParameter colorResolution = new ClampedIntParameter(8, 1, 64);

    Material m_Material;

    public bool IsActive() => m_Material != null && intensity.value > 0f;

    public override CustomPostProcessInjectionPoint injectionPoint => CustomPostProcessInjectionPoint.AfterPostProcess;

    const string kShaderName = "Shader Graphs/ParaboxToonShader";

    public override void Setup()
    {
        Shader shader = Shader.Find(kShaderName);
        if (shader != null)
        {
            m_Material = new Material(shader);
        }
        else
        {
            Debug.LogError($"Unable to find shader '{kShaderName}'. Post Process Volume ParaboxToon is unable to load.");
        }
    }

    public override void Render(CommandBuffer cmd, HDCamera camera, RTHandle source, RTHandle destination)
    {
        if (m_Material == null) return;

#if UNITY_EDITOR
        if (camera.camera.cameraType != CameraType.Game || !EditorApplication.isPlaying)
        {
            cmd.Blit(source, destination, 0, 0);
            return;
        }
#endif

        m_Material.SetTexture("_MainTex", source);

        m_Material.SetFloat("_Intensity", intensity.value);
        m_Material.SetFloat("_Normal_Threshold", outlineNormalThreshold.value);
        m_Material.SetFloat("_DitherSpread", ditherSpread.value);
        m_Material.SetFloat("_DitherSize", ditherSize.value);
        m_Material.SetInt("_Color_Resolution", colorResolution.value);

        HDUtils.BlitCameraTexture(cmd, source, destination, m_Material, 0);
        cmd.Blit(source, destination, 0, 0);
        
        HDUtils.DrawFullScreen(cmd, m_Material, destination);
    }

    public override void Cleanup()
    {
        CoreUtils.Destroy(m_Material);
    }
}
