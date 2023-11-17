using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(DeathConfig), menuName = "Weather Guardian/Behaviours/Configs/Death Config")]
    public class DeathConfig : ScriptableObject
    {
        [field: Tooltip("Difine capsule collider center when enter in death state.")]
        [field: SerializeField] public Vector3 CapsuleCenter { get; private set; } = Vector3.up;

        [field: Tooltip("Difine capsule collider height when enter in death state.")]
        [field: SerializeField] public float CapsuleHeight { get; private set; } = 2;
    }
}