using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(DashConfig), menuName = "TesisBand/Behaviours/Configs/Dash Config")]
    public class DashConfig : ScriptableObject
    {
        [field: Tooltip("Use umbrella when dash end")]
        [field: SerializeField] public bool UseUmbrella { get; private set; } = true;

        [field: Tooltip("Cooldown to use the dash.")]
        [field: SerializeField] public float Cooldown { get; private set; } = 1.0f;

        [field: Tooltip("Duration in seconds of the dash.")]
        [field: SerializeField] public float Duration { get; private set; } = 1.0f;

        [field: Tooltip("Distance to travel with dash.")]
        [field: SerializeField] public float Distance { get; private set; } = 100.0f;

        [field: Tooltip("Difine velocity behaviour of the dash in time life.")]
        [field: SerializeField] public AnimationCurve VelocityBehaviourCurve { get; private set; } = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 1.0f), new Keyframe(1.0f, 0.0f) });
    }
}