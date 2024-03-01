using UnityEngine;

public class SettingsMusicAndSFXSlidersConfig : MonoBehaviour
{
    FMOD.Studio.Bus Music;

    FMOD.Studio.Bus SFX;

    private float musicVolume = 0.5f;

    private float SFXVolume = 0.5f;

    private void Awake()
    {
        Music = FMODUnity.RuntimeManager.GetBus("bus:/Music");

        SFX = FMODUnity.RuntimeManager.GetBus("bus:/SFX");        
    }

    public void MusicVolumeLevel(float newMusicVolume) 
    {
        musicVolume = newMusicVolume;
    }

    public void SFXVolumeLevel(float newSFXVolume) 
    {
        SFXVolume = newSFXVolume;
    }
}