using UnityEngine;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// Dynamically colours a pendulum depending on it's angular displacement.
    /// </summary>
    [RequireComponent(typeof(TorsionalOscillator))]
    [RequireComponent(typeof(MeshRenderer))]
    public class DynamicPendulumColour : MonoBehaviour
    {
        private TorsionalOscillator torsionalOscillator;
        private Renderer meshRenderer;
        private Material dynamicPendulumMaterial;

        /// <summary>
        /// Define the required variables.
        /// </summary>
        private void Start()
        {
            torsionalOscillator = GetComponent<TorsionalOscillator>();
            meshRenderer = GetComponent<MeshRenderer>();
            dynamicPendulumMaterial = meshRenderer.material;
        }

        /// <summary>
        /// Update the colour of the pendulum, such that it is determined by the angular displacement of the pendulum.
        /// </summary>
        private void FixedUpdate()
        {
            float angle = torsionalOscillator.angularDisplacementMagnitude;

            Color color = Color.green;
            float upperAmplitude = 20f; // Approximately the upper limit of the angle amplitude within regular use
            float ratio = angle / upperAmplitude;

            float r = 2f * (1f - Mathf.Clamp(ratio, 0.5f, 1f));
            float g = 2f * Mathf.Clamp(ratio, 0f, 0.5f);
            float b = 1f;

            color.r = r;
            color.g = g;
            color.b = b;
            UnityEngine.Gizmos.color = color;

            dynamicPendulumMaterial.color = color;
        }
    }
}