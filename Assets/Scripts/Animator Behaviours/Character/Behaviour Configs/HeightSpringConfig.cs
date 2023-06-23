using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(HeightSpringConfig), menuName = "TesisBand/Behaviours/Configs/Height Spring Config")]
    public class HeightSpringConfig : ScriptableObject
    {
        [field: Header("Height Spring:")]
        [field: Space(5)]

        [field: Tooltip("Desired distance to ground (Note, this is distance from the original raycast position (currently centre of transform)).")]
        [field: SerializeField] public float RideHeight { get; private set; } = 2.0f;

        [field: Tooltip("Layer for ray check.")]
        [field: SerializeField] public LayerMask RayLayerMask { get; private set; }

        [field: Tooltip("Max distance of raycast to ground (Note, this should be greater than the rideHeight).")]
        [field: SerializeField] public float RayCastLength { get; private set; } = 3.0f;

        [field: Tooltip("Radius of the ray (is a sphere ray cast).")]
        [field: SerializeField] public float RayCastRadius { get; private set; } = 0.25f;

        [field: Tooltip("Strength of spring...")]
        [field: SerializeField] public float Strength { get; private set; } = 200.0f;

        [field: Tooltip("Dampener of spring.")]
        [field: SerializeField] public float Damper { get; private set; } = 10.0f;
    }
}