using CryingOnion.Tools.Runtime;
using System;
using UnityEngine;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// A dampened oscillator using the objects transform local position.
    /// </summary>
    [DisallowMultipleComponent]

    public class Oscillator : MonoBehaviour
    {
        [Tooltip("The local position about which oscillations are centered.")]
        [field: SerializeField] public Vector3 EquilibriumPosition { get; set; } = Vector3.zero;

        [Tooltip("The axes over which the oscillator applies force. Within range [0, 1].")]
        [field: SerializeField] public Vector3 ForceScale { get; set; } = Vector3.one;

        [Tooltip("The greater the stiffness constant, the lesser the amplitude of oscillations.")]
        [field: SerializeField] public float Stiffness { get; private set; } = 100.0f;

        [Tooltip("The greater the damper constant, the faster that oscillations will dissapear.")]
        [field: SerializeField] public float Damper { get; private set; } = 6.0f;

        [Tooltip("The greater the mass, the lesser the amplitude of oscillations.")]
        [field: SerializeField] public float Mass { get; private set; } = 1.0f;

        [SerializeField] private Gradient gradient;

        private Vector3 previousDisplacement = Vector3.zero;
        private Vector3 previousVelocity = Vector3.zero;

        private readonly Guid id = Guid.NewGuid();

        private void Awake() => EquilibriumPosition = transform.position;

        /// <summary>
        /// Update the position of the oscillator, by calculating and applying the restorative force.
        /// </summary>
        private void FixedUpdate()
        {
            Vector3 restoringForce = CalculateRestoringForce();
            ApplyForce(restoringForce);
        }

        /// <summary>
        /// Returns the damped restorative force of the oscillator.
        /// The magnitude of the restorative force is 0 at the equilibrium position and maximum at the amplitude of the oscillation.
        /// </summary>
        /// <returns>Damped restorative force of the oscillator.</returns>
        private Vector3 CalculateRestoringForce()
        {
            /* Displacement from the rest point. Displacement is the difference in position. */
            Vector3 displacement = transform.position - EquilibriumPosition;
            Vector3 deltaDisplacement = displacement - previousDisplacement;
            previousDisplacement = displacement;

            /* Kinematics. Velocity is the change-in-position over time. */
            Vector3 velocity = deltaDisplacement / Time.fixedDeltaTime;
            Vector3 force = HookesLaw(displacement, velocity);

            return (force);
        }

        /// <summary>
        /// Returns the damped Hooke's force for a given displacement and velocity.
        /// </summary>
        /// <param name="displacement">The displacement of the oscillator from the equilibrium position.</param>
        /// <param name="velocity">The local velocity of the oscillator.</param>
        /// <returns>Damped Hooke's force</returns>
        private Vector3 HookesLaw(Vector3 displacement, Vector3 velocity)
        {
            /* Damped Hooke's law */
            Vector3 force = (Stiffness * displacement) + (Damper * velocity);

            /* Take the negative of the force, since the force is restorative (attractive) */
            force = -force;
            return (force);
        }

        /// <summary>
        /// Adds a force to the oscillator. Updates the transform's local position.
        /// </summary>
        /// <param name="force">The force to be applied.</param>
        public void ApplyForce(Vector3 force)
        {
            Rigidbody rb = GetComponent<Rigidbody>();

            if (rb != null)
                rb.AddForce(Vector3.Scale(force, ForceScale));
            else
            {
                Vector3 displacement = CalculateDisplacementDueToForce(force);
                transform.position += Vector3.Scale(displacement, ForceScale);
            }
        }

        /// <summary>
        /// Returns the displacement that results from applying a force over a single fixed update.
        /// </summary>
        /// <param name="force">The causative force.</param>
        /// <returns>Displacement over a single fixed update.</returns>
        private Vector3 CalculateDisplacementDueToForce(Vector3 force)
        {
            /* Newton's second law. */
            Vector3 acceleration = force / Mass;

            /* Kinematics. Acceleration is the change in velocity over time. */
            Vector3 deltaVelocity = acceleration * Time.fixedDeltaTime;

            /* Calculating the updated velocity. */
            Vector3 velocity = deltaVelocity + previousVelocity;
            previousVelocity = velocity;

            /* Kinematics. Velocity is the change-in-position over time. */
            Vector3 displacement = velocity * Time.fixedDeltaTime;
            return (displacement);
        }

        /// <summary>
        /// Draws the oscillator bob (sphere) and the equilibrium (wire sphere).
        /// </summary>
        //void OnDrawGizmos()
        private void LateUpdate()
        {
            if (OhMyGizmos.Enabled)
            {
                Vector3 bob = transform.position;
                Vector3 equilibrium = EquilibriumPosition;

                /* Draw (solid) bob position. */
                /* Color goes from green (0,1,0,0) to yellow (1,1,0,0) to red (1,0,0,0). */
                /* Approximately the upper limit of the amplitude within regular use. */
                float upperAmplitude = Stiffness * Mass / (3f * 100f);
                Color color = gradient.Evaluate(Mathf.Clamp(Vector3.Magnitude(bob - equilibrium) * upperAmplitude, 0f, 1.0f));

                /* Draw (wire) equilibrium position. */
                OhMyGizmos.Cube(Matrix4x4.TRS(equilibrium, transform.rotation, Vector3.one * 0.3f), color);

                OhMyGizmos.Sphere(bob, 0.2f, color);
                OhMyGizmos.Line(id, bob, equilibrium, gradient);
            }
        }
    }
}