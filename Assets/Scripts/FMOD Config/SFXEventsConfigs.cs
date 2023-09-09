using UnityEngine;

namespace WeatherGuardian.FMOD.Configs 
{
    [CreateAssetMenu(fileName = nameof(SFXEventsConfigs), menuName = "TesisBand/FMOD/Configs/SFXEventsConfigs")]
    public class SFXEventsConfigs : ScriptableObject
    {
        [field: SerializeField] public FMODUnity.EventReference footstepSfx { get; private set; }
        [field: SerializeField] public FMODUnity.EventReference landingSfx { get; private set; }
        [field: SerializeField] public FMODUnity.EventReference jumpSfx { get; private set; }
        [field: SerializeField] public FMODUnity.EventReference openUmbrellaSfx { get; private set; }
        [field: SerializeField] public FMODUnity.EventReference closeUmbrellaSfx { get; private set; }
        [field: SerializeField] public FMODUnity.EventReference dashSfx { get; private set; }
    }
}