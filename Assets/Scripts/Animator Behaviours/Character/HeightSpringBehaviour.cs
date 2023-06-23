using CryingOnion.OscillatorSystem;
using WeatherGuardian.Behaviours.Configs;
using WeatherGuardian.PlatformObjects;
using UnityEngine;
using System;
using CryingOnion.GizmosRT.Runtime;

namespace WeatherGuardian.Behaviours
{
    public class HeightSpringBehaviour : StateMachineBehaviour
    {
        [Header("Height Spring:")]
        [SerializeField] private HeightSpringConfig heightSpringConfig;

        private Rigidbody body;

        public Oscillator Oscillator { get; set; }

        public Rigidbody Body => body;

        public float RideHeight => heightSpringConfig.RideHeight;

        public bool ShouldMaintainHeight { get; set; } = true;

        public Vector3 GravitationalForce => Physics.gravity * body.mass;

        public float LastGroundedTime { get; private set; }

        public (bool grounded, RaycastHit rayHit) GroundedInfo { get; private set; }

        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            if (body == null)
                body = animator.GetComponent<Rigidbody>();
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            var data = RaycastToGround(body.worldCenterOfMass, Physics.gravity * body.mass);
            SetPlatform(data.rayhit, body.transform);

            bool grounded = CheckIfGrounded(data.rayHitGround, data.rayhit);

            LastGroundedTime = grounded ? 0 : LastGroundedTime + Time.fixedDeltaTime;

            Vector3 oscillationForce = Vector3.zero;

            if (data.rayHitGround && ShouldMaintainHeight)
                oscillationForce = MaintainHeight(body, data.rayhit, Physics.gravity,  Physics.gravity * body.mass);

            if (Oscillator != null)
                Oscillator.ApplyForce(oscillationForce);

            GroundedInfo = (grounded, data.rayhit);
        }

        /// <summary>
        /// Use the result of a Raycast to determine if the capsules distance from the ground is sufficiently close to the desired ride height such that the character can be considered 'grounded'.
        /// </summary>
        /// <param name="rayHitGround">Whether or not the Raycast hit anything.</param>
        /// <param name="rayHit">Information about the ray.</param>
        /// <returns>Whether or not the player is considered grounded.</returns>
        private bool CheckIfGrounded(in bool rayHitGround, in RaycastHit rayHit)
        {
            bool grounded;

            if (rayHitGround == true)
                grounded = rayHit.distance <= heightSpringConfig.RideHeight * 1.7f; // 1.3f allows for greater leniancy (as the value will oscillate about the rideHeight).
            else
                grounded = false;

            return grounded;
        }

        /// <summary>
        /// Perfom raycast towards the ground.
        /// </summary>
        /// <returns>Whether the ray hit the ground, and information about the ray.</returns>
        private (bool rayHitGround, RaycastHit rayhit) RaycastToGround(in Vector3 position, in Vector3 direction)
        {
            RaycastHit rayHit;
            Ray rayToGround = new Ray(position, direction);

            //bool rayHitGround = Physics.Raycast(rayToGround, out rayHit, heightSpringConfig.RayCastLength, heightSpringConfig.TerrainLayer.value);
            bool rayHitGround = Physics.SphereCast(rayToGround, heightSpringConfig.RayCastRadius, out rayHit, heightSpringConfig.RayCastLength, heightSpringConfig.RayLayerMask);

            return (rayHitGround, rayHit);
        }

        /// <summary>
        /// Set the transform parent to be the result of RaycastToGround.
        /// If the raycast didn't hit, then unset the transform parent.
        /// </summary>
        /// <param name="rayHit">The rayHit towards the platform.</param>
        private void SetPlatform(in RaycastHit rayHit, in Transform transform)
        {
            try
            {
                RigidPlatform rigidPlatform = rayHit.transform.GetComponent<RigidPlatform>();
                RigidParent rigidParent = rigidPlatform.rigidParent;
                transform.SetParent(rigidParent.transform);
            }
            catch
            {
                transform.SetParent(null);
            }
        }

        /// <summary>
        /// Determines the relative velocity of the character to the ground beneath,
        /// Calculates and applies the oscillator force to bring the character towards the desired ride height.
        /// Additionally applies the oscillator force to the squash and stretch oscillator, and any object beneath.
        /// </summary>
        /// <param name="rayHit">Information of ray cast to the ground</param>
        /// <param name="rayDir"> Ray direction.</param>
        /// <param name="gravitationalForce">Gravity direction and force.</param>
        /// <param name="body">Rigidbody to affect.</param>
        /// <returns>The oscillation force</returns>
        private Vector3 MaintainHeight(in Rigidbody body, in RaycastHit rayHit, in Vector3 rayDir, in Vector3 gravitationalForce)
        {
            Vector3 vel = body.velocity;
            Vector3 otherVel = Vector3.zero;
            Rigidbody hitBody = rayHit.rigidbody;

            if (hitBody != null)
                otherVel = hitBody.velocity;

            float rayDirVel = Vector3.Dot(rayDir, vel);
            float otherDirVel = Vector3.Dot(rayDir, otherVel);

            float relVel = rayDirVel - otherDirVel;
            float currHeight = rayHit.distance - heightSpringConfig.RideHeight;
            float springForce = (currHeight * heightSpringConfig.Strength) - (relVel * heightSpringConfig.Damper);
            Vector3 maintainHeightForce = -gravitationalForce + springForce * Vector3.down;
            Vector3 ocillationForce = springForce * Vector3.down;

            body.AddForce(maintainHeightForce);

            // Apply force to objects beneath
            if (hitBody != null)
                hitBody.AddForceAtPosition(-maintainHeightForce, rayHit.point);

            return ocillationForce;
        }
    }
}