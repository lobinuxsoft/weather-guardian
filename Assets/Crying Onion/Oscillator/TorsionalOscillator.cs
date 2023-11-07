using CryingOnion.Tools.Runtime;
using CryingOnion.Utils;
using System;
using UnityEngine;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// A dampened torsional oscillator using the objects transform local rotation and the rigibody.
    /// Unfortunately, the option of not requiring a rigidbody was proving more difficult than expected,
    /// due to difficulty working with quaternions whilst calculating angular velocity, torque and angular
    /// displacement to apply.
    /// </summary>
    [DisallowMultipleComponent]
    public class TorsionalOscillator : MonoBehaviour
    {
        [Tooltip("The local rotation about which oscillations are centered.")]
        [field: SerializeField] public Vector3 LocalEquilibriumRotation { get; set; } = Vector3.zero;

        [Tooltip("The axes over which the oscillator applies torque. Within range [0, 1].")]
        [field: SerializeField] public Vector3 TorqueScale { get; set; } = Vector3.one;

        [Tooltip("The greater the stiffness constant, the lesser the amplitude of oscillations.")]
        [field: SerializeField] public float Stiffness { get; set; } = 100.0f;

        [Tooltip("The greater the damper constant, the faster that oscillations will dissapear.")]
        [field: SerializeField] public float Damper { get; private set; } = 5.0f;

        [Tooltip("The center about which rotations should occur.")]
        [field: SerializeField] public Vector3 LocalPivotPosition { get; set; } = Vector3.zero;

        [SerializeField] private Gradient arcGradient;

        public Vector3 RotAxis => rotAxis;

        public float angularDisplacementMagnitude;

        private Vector3 rotAxis;

        private Rigidbody body;

        private readonly Guid circleId = Guid.NewGuid();
        private readonly Guid arcId = Guid.NewGuid();

        /// <summary>
        /// Get the rigidbody component.
        /// </summary>
        private void Start()
        {
            body = GetComponent<Rigidbody>();
            body.centerOfMass = LocalPivotPosition;
        }

        /// <summary>
        /// Set the center of rotation.
        /// Update the rotation of the oscillator, by calculating and applying the restorative torque.
        /// </summary>
        private void FixedUpdate()
        {
            Vector3 restoringTorque = CalculateRestoringTorque();
            ApplyTorque(restoringTorque);

            body.centerOfMass = LocalPivotPosition;
        }

        /// <summary>
        /// Returns the damped restorative torque of the oscillator.
        /// The magnitude of the restorative torque is 0 at the equilibrium rotation and maximum at the amplitude of the oscillation.
        /// </summary>
        /// <returns>Damped restorative torque of the oscillator.</returns>
        private Vector3 CalculateRestoringTorque()
        {
            Quaternion deltaRotation = MathsUtils.ShortestRotation(transform.localRotation, Quaternion.Euler(LocalEquilibriumRotation));
            deltaRotation.ToAngleAxis(out angularDisplacementMagnitude, out rotAxis);
            Vector3 angularDisplacement = angularDisplacementMagnitude * Mathf.Deg2Rad * rotAxis.normalized;
            Vector3 torque = AngularHookesLaw(angularDisplacement, body.angularVelocity);
            return torque;
        }

        /// <summary>
        /// Returns the damped Hooke's torque for a given angularDisplacement and angularVelocity.
        /// </summary>
        /// <param name="angularDisplacement">The angular displacement of the oscillator from the equilibrium rotation.</param>
        /// <param name="angularVelocity">The local angular velocity of the oscillator.</param>
        /// <returns>Damped Hooke's torque</returns>
        private Vector3 AngularHookesLaw(Vector3 angularDisplacement, Vector3 angularVelocity)
        {
            Vector3 torque = (Stiffness * angularDisplacement) + (Damper * angularVelocity); // Damped angular Hooke's law
            torque = -torque; // Take the negative of the torque, since the torque is restorative (attractive)
            return (torque);
        }

        /// <summary>
        /// Adds a torque to the oscillator using the rigidbody.
        /// </summary>
        /// <param name="torque">The torque to be applied.</param>
        private void ApplyTorque(Vector3 torque) => body.AddTorque(Vector3.Scale(torque, TorqueScale));

        /// <summary>
        /// Draws the pivot of rotation (wire sphere), the oscillator bob (sphere) and the equilibirum (wire sphere).
        /// </summary>
        //void OnDrawGizmos()
        private void LateUpdate()
        {
            if (OhMyGizmos.Enabled)
            {
                Vector3 bob = transform.position;
                Vector3 axis = rotAxis.normalized;
                float angle = angularDisplacementMagnitude;

                // Draw (wire) pivot position
                Vector3 pivotPosition = transform.TransformPoint(Vector3.Scale(LocalPivotPosition, MathsUtils.Invert(transform.localScale)));

                // Color goes from green (0,1,0,0) to yellow (1,1,0,0) to red (1,0,0,0).
                float upperAmplitude = 90f; // Approximately the upper limit of the angle amplitude within regular use
                Color color = arcGradient.Evaluate(Mathf.Clamp(angle / upperAmplitude, 0f, 1.0f));

                // Draw (arc) angle to equilibrium
                Vector3 equilibrium = OhMyGizmos.DrawArc(arcId, pivotPosition, bob, axis, 0f, -angle / 360f, 32, arcGradient);

                OhMyGizmos.Circle(circleId, pivotPosition, Quaternion.LookRotation(pivotPosition - equilibrium), Color.white, 0.5f);

                // Draw (solid) bob position
                OhMyGizmos.Sphere(bob, 0.2f, color);

                // Draw (wire) equilibrium position
                OhMyGizmos.Cube(Matrix4x4.TRS(equilibrium, Quaternion.identity, Vector3.one * 0.3f), color);
            }
        }
    }
}