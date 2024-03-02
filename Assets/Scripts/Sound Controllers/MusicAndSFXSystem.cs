using UnityEngine;

using UnityEngine.UI;

public class MusicAndSFXSystem : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1.0f)] private float InitialMusicVolumeValue;

    [SerializeField] [Range(0.1f, 1.0f)] private float InitialSFXVolumeValue;
    
    [SerializeField] private Slider musicSlider;

    [SerializeField] private Slider SFXSlider;

    [SerializeField] private string musicBusRoute;

    [SerializeField] private string SFXBusRoute;

    private static FMOD.Studio.Bus MusicBus;

    private static FMOD.Studio.Bus SFXBus;

    private static bool firstSetting = true;

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

    public void UpdateSystemWithSliders()
    {
        UpdateVolumes(musicSlider.value, SFXSlider.value);

        UpdateBusesVolumes(musicVolume, SFXVolume);        
    }

    private void UpdateSystem(float musicVolume, float SFXVolume) 
    {
        UpdateVolumes(musicVolume, SFXVolume);

        UpdateBusesVolumes(musicVolume, SFXVolume);        
    }

    private void UpdateVolumes(float newMusicVolume, float newSFXVolume) 
    {
        musicVolume = newMusicVolume;

        SFXVolume = newSFXVolume;
    }

    public void SetMusicVolumeLevel(float newMusicVolume) 
    {
        musicVolume = newMusicVolume;        
    }

    public void SetSFXVolumeLevel(float newSFXVolume) 
    {
        SFXVolume = newSFXVolume;        
    }

    private void UpdateBusesVolumes(float newMusicBusVolume, float newSFXBusVolume) 
    {
        MusicBus.setVolume(newMusicBusVolume);

        SFXBus.setVolume(newSFXBusVolume);
    }
}