using WeatherGuardian.Behaviours.Configs;
using UnityEngine;

namespace WeatherGuardian.Behaviours
{
    public class StompBehaviour : StateMachineBehaviour
    {
        int stompHash = Animator.StringToHash("STOMP");

        [SerializeField] StompConfig stompConfig;

        HeightSpringBehaviour heightSpringBehaviour;
        AirUmbrellaBehaviour airUmbrellaBehaviour;


        // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
        override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            Vector3 force = Vector3.down * stompConfig.StompForce;
            heightSpringBehaviour.Body.linearDamping = 0;
            heightSpringBehaviour.Body.AddForce(force, ForceMode.Impulse);
        }

        // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
        override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            float stompDot = Vector3.Dot(heightSpringBehaviour.GroundedInfo.rayHit.normal, Vector3.up);

            GravityInfluence(animator);

            if (stompDot <= 0) return;

            heightSpringBehaviour.ShouldMaintainHeight = false;

            Debug.Log($"Stomp to something valid? {(1 << heightSpringBehaviour.GroundedInfo.rayHit.transform.gameObject.layer) == stompConfig.LayerMask.value}");

            Vector3 force = heightSpringBehaviour.GroundedInfo.rayHit.normal * stompConfig.BounceForce;

            heightSpringBehaviour.Body.AddForce(force, ForceMode.Impulse);
            heightSpringBehaviour.Oscillator.ApplyForce(force);
            heightSpringBehaviour.GroundedInfo.rayHit.rigidbody?.AddForceAtPosition(-force, heightSpringBehaviour.GroundedInfo.rayHit.point);

            float bodyVelDot = Vector3.Dot(heightSpringBehaviour.Body.linearVelocity, Vector3.up);
            animator.SetBool(stompHash, !(bodyVelDot > 0.0f));
        }

        public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
        {
            animator.SetBool(stompHash, false);
        }

        public void Stomp(Animator animator)
        {
            if(heightSpringBehaviour == null)
                heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();

            if(airUmbrellaBehaviour == null)
                airUmbrellaBehaviour = animator.GetBehaviour<AirUmbrellaBehaviour>();

            if(!heightSpringBehaviour.GroundedInfo.grounded)
            {
                animator.SetBool(stompHash, true);
                airUmbrellaBehaviour.UmbrellaCloseForced(animator);
            }
        }

        private void GravityInfluence(Animator animator)
        {
            if (heightSpringBehaviour == null || heightSpringBehaviour.GroundedInfo.grounded) return;

            heightSpringBehaviour.Body.linearVelocity += heightSpringBehaviour.GravitationalForce * stompConfig.GravityMultiplier * Time.fixedDeltaTime;
        }
    }
}