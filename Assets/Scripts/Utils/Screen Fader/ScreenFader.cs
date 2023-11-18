using System;
using System.Threading.Tasks;
using UnityEngine;
using WeatherGuardian.Utils;

public class ScreenFader : SingletonPersistent<ScreenFader>
{
    [SerializeField] private Material material;
    [SerializeField] private AnimationCurve fadeInBehaviour;
    [SerializeField, Min(0)] private float fadeInDuration = 1;
    [SerializeField] private AnimationCurve fadeOutBehaviour;
    [SerializeField, Min(0)] private float fadeOutDuration = 1;

    public async Task FadeIn(Action fadeEndAction = null)
    {
        float lerp = 0;

        while (lerp < fadeInDuration)
        {
            lerp += Time.unscaledDeltaTime;

            material.SetFloat("_FadeIntensity", fadeInBehaviour.Evaluate(lerp/fadeInDuration));

            await Task.Yield();
        }

        material.SetFloat("_FadeIntensity", fadeInBehaviour.Evaluate(lerp / fadeInDuration));

        fadeEndAction?.Invoke();
    }

    public async Task FadeOut(Action fadeEndAction = null)
    {
        float lerp = 0;

        while (lerp < fadeOutDuration)
        {
            lerp += Time.unscaledDeltaTime;

            material.SetFloat("_FadeIntensity", fadeOutBehaviour.Evaluate(lerp / fadeOutDuration));

            await Task.Yield();
        }

        material.SetFloat("_FadeIntensity", fadeOutBehaviour.Evaluate(lerp / fadeOutDuration));

        fadeEndAction?.Invoke();
    }
}