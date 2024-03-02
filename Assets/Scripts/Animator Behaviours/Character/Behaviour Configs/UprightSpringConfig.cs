using UnityEngine;

namespace WeatherGuardian.Behaviours
{
    [CreateAssetMenu(fileName = nameof(UprightSpringConfig), menuName = "Weather Guardian/Behaviours/Configs/Upright Spring Config")]
    public class UprightSpringConfig : ScriptableObject
    {
        [field: Header("Upright Spring:")]
        [field: SerializeField] public float Strength { get; private set; } = 40.0f;
        [field: SerializeField] public float Damper { get; private set; } = 5.0f;
    }
}