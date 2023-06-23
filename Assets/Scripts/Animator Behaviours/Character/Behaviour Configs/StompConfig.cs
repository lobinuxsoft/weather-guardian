using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(StompConfig), menuName = "TesisBand/Behaviours/Configs/Stomp Config")]
    public class StompConfig : ScriptableObject
    {
        [field: Header("Stomp:")]

        [Tooltip("The force with which it falls.")]
        [field: SerializeField] public float StompForce { get; private set; } = 16.0f;

        [Tooltip("The force with which it bounces.")]
        [field: SerializeField] public float BounceForce { get; private set; } = 4.0f;

        [Tooltip("Mask with which it is detected if it is valid to interact or not.")]
        [field: SerializeField] public LayerMask LayerMask { get; private set; }
    }
}