using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(MoveConfig), menuName = "Weather Guardian/Behaviours/Configs/Move Config")]
    public class MoveConfig : ScriptableObject
    {
        [field: Header("Movement:")]
        [field: SerializeField] public float MaxSpeed { get; private set; } = 10.0f;
        [field: SerializeField] public float Acceleration { get; private set; } = 400.0f;
        [field: SerializeField] public float MaxAccelForce { get; private set; } = 350.0f;
        [field: SerializeField] public float LeanFactor { get; private set; } = 0.45f;
        [field: SerializeField] public AnimationCurve AccelerationFactorFromDot { get; private set; } = new AnimationCurve(new Keyframe[] { new Keyframe(-1, 2), new Keyframe(0, 1), new Keyframe(1, 1) });
        [field: SerializeField] public AnimationCurve MaxAccelerationForceFactorFromDot { get; private set; } = new AnimationCurve(new Keyframe[] { new Keyframe(-1, 2), new Keyframe(0, 1), new Keyframe(1, 1) });
        [field: SerializeField] public Vector3 MoveForceScale { get; private set; } = new Vector3(1f, 0f, 1f);
        [field: SerializeField] public float SprintMultiplier { get; private set; } = 2.0f;
        [field: SerializeField] public float TimerToSprint { get; private set; } = 1.5f;

        [field: Tooltip("Layer for ray check.")]
        [field: SerializeField] public LayerMask RayLayerMask { get; private set; }
    }
}