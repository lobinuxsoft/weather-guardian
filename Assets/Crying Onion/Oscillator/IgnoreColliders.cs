using System.Collections.Generic;
using UnityEngine;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// Makes this collider ignore all collisions with other specified colliders.
    /// </summary>
    public class IgnoreColliders : MonoBehaviour
    {
        [SerializeField] private List<Collider> collidersToIgnore;
        private Collider thisCollider;

        /// <summary>
        /// Turn off collisions between this collider and other collidersToIgnore.
        /// </summary>
        private void Start()
        {
            thisCollider = this.GetComponent<Collider>();

            foreach (Collider otherCollider in collidersToIgnore)
                Physics.IgnoreCollision(thisCollider, otherCollider);
        }
    }
}