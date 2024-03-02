using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(JumpConfig), menuName = "Weather Guardian/Behaviours/Configs/Jump Config")]
    public class JumpConfig : ScriptableObject
    {
        [field: Header("Jump:")]
        [field: SerializeField] public float JumpForceFactor { get; private set; } = 16.0f;
        [field: SerializeField] public float CoyoteTime { get; private set; } = 0.25f;
    }
}