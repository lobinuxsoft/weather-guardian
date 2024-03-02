using UnityEngine;

using UnityEngine.UI;

public class MusicAndSFXSystem : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1.0f)] private float InitialMusicVolumeValue;

    [SerializeField] [Range(0.1f, 1.0f)] private float InitialSFXVolumeValue;
    
    [SerializeField] private Slider musicSlider;

    [SerializeField] private Slider sfxSlider;

    [SerializeField] private string musicBusRoute;

    [SerializeField] private string SFXBusRoute;

    private static FMOD.Studio.Bus MusicBus;

    private static FMOD.Studio.Bus SFXBus;

    private static bool firstSetting = false;

    private static float musicVolume;

    private static float SFXVolume;

    private void Awake()
    {
        AwakeInitialConfiguration();
    }

    private void Start()
    {
        StartInitialConfiguration();
    }

    private void AwakeInitialConfiguration()
    {
        if (firstSetting)
        {
            MusicBus = FMODUnity.RuntimeManager.GetBus(musicBusRoute);

            SFXBus = FMODUnity.RuntimeManager.GetBus(SFXBusRoute);
        }        
    }

    private void StartInitialConfiguration() 
    {
        if (firstSetting)
        {
            UpdateSystem(InitialMusicVolumeValue, InitialSFXVolumeValue);

            firstSetting = !firstSetting;
        }
    }

    private void UpdateSystem(float musicVolume, float SFXVolume) 
    {
        UpdateVolumes(musicVolume, SFXVolume);

        UpdateBusesVolumes(musicVolume, SFXVolume);

        UpdateSlidersValues(musicVolume, SFXVolume);
    }

    private void UpdateVolumes(float newMusicVolume, float newSFXVolume) 
    {
        musicVolume = newMusicVolume;

        SFXVolume = newSFXVolume;
    }

    public void SetMusicVolumeLevel(float newMusicVolume) 
    {
        musicVolume = newMusicVolume;

        UpdateSystem(musicVolume, SFXVolume);
    }

    public void SetSFXVolumeLevel(float newSFXVolume) 
    {
        SFXVolume = newSFXVolume;

        UpdateSystem(musicVolume, SFXVolume);
    }

    private void UpdateBusesVolumes(float newMusicBusVolume, float newSFXBusVolume) 
    {
        MusicBus.setVolume(newMusicBusVolume);

        SFXBus.setVolume(newSFXBusVolume);
    }

    private void UpdateSlidersValues(float newMusicSliderValue, float newSFXSliderValue)
    {
        musicSlider.value = newMusicSliderValue;

        sfxSlider.value = newSFXSliderValue;
    }
}