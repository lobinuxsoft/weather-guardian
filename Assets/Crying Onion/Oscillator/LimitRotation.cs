using UnityEngine;
using CryingOnion.Utils;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// Limits the range of rotation of this rigid body.
    /// </summary>
    public class LimitRotation : MonoBehaviour
    {
        // +- Range of rotations for each respective axis.
        [SerializeField] private Vector3 maxLocalRotation = Vector3.one * 360f;

        private Rigidbody body;

        /// <summary>
        /// Define the rigid body.
        /// </summary>
        private void Start() => body = GetComponent<Rigidbody>();

        /// <summary>
        /// Clamp the rotation to be less than the desired maxLocalRotation.
        /// </summary>
        private void FixedUpdate()
        {
            Quaternion clampedLocalRot = MathsUtils.ClampRotation(transform.localRotation, maxLocalRotation);
            body.MoveRotation(clampedLocalRot);
        }
    }
}