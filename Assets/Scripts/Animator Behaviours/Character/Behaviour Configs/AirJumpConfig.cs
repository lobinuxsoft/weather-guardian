using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(AirJumpConfig), menuName = "Weather Guardian/Behaviours/Configs/Air Jump Config")]
    public class AirJumpConfig : ScriptableObject
    {
        [field: Header("Air Jump:")]

        [field: SerializeField] public float JumpForceFactor { get; private set; } = 16.0f;
        [field: SerializeField] public float CoyoteTime { get; private set; } = 0.25f;
        [field: SerializeField] public int JumpAmount { get; private set; } = 1;
    }
}