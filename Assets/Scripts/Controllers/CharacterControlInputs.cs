using CryingOnion.OscillatorSystem;
using CryingOnion.Tools.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;
using WeatherGuardian.Behaviours;

namespace Game.InputsController
{
    public class CharacterControlInputs : MonoBehaviour
    {
        [Header("Only for Debug")]
        [SerializeField] InputActionReference debugAction;

        [Space(5)]
        [Header("Character Inputs")]
        [SerializeField] InputActionReference moveAction;
        [SerializeField] InputActionReference jumpAction;
        [SerializeField] InputActionReference umbrellaAction;
        //[SerializeField] InputActionReference stompAction;
        [SerializeField] InputActionReference dashAction;

        [SerializeField] Animator animator;
        [SerializeField] Oscillator oscillator;

        private HeightSpringBehaviour heightSpringBehaviour;
        private UprightSpringBehaviour uprightSpringBehaviour;
        private MoveBehaviour moveBehaviour;
        private JumpBehaviour jumpBehaviour;
        private AirJumpBehaviour doubleJumpBehaviour;
        private AirBehaviour airBehaviour;
        private AirUmbrellaBehaviour airUmbrellaBehaviour;
        //private StompBehaviour stompBehaviour;
        private DashBehaviour dashBehaviour;

        private void OnEnable()
        {
            debugAction.action.started += SwitchDebug;
            debugAction.action.Enable();

            moveAction.action.performed += MoveAction;
            moveAction.action.canceled += MoveAction;

            moveAction.action.performed += AirMoveAction;

            jumpAction.action.started += JumpAction;
            umbrellaAction.action.started += UmbrellaAction;
            //stompAction.action.started += StompAction;
            dashAction.action.started += DashAction;

            moveAction.action.Enable();
            jumpAction.action.Enable();
            umbrellaAction.action.Enable();
            //stompAction.action.Enable();
            dashAction.action.Enable();

            heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();
            heightSpringBehaviour.Oscillator = oscillator;

            uprightSpringBehaviour = animator.GetBehaviour<UprightSpringBehaviour>();

            moveBehaviour = animator.GetBehaviour<MoveBehaviour>();
            jumpBehaviour = animator.GetBehaviour<JumpBehaviour>();
            doubleJumpBehaviour = animator.GetBehaviour<AirJumpBehaviour>();
            airBehaviour = animator.GetBehaviour<AirBehaviour>();
            airUmbrellaBehaviour = animator.GetBehaviour<AirUmbrellaBehaviour>();
            //stompBehaviour = animator.GetBehaviour<StompBehaviour>();
            dashBehaviour = animator.GetBehaviour<DashBehaviour>();
        }

        private void OnDisable()
        {
            debugAction.action.started -= SwitchDebug;
            debugAction.action.Disable();

            moveAction.action.performed -= MoveAction;
            moveAction.action.canceled -= MoveAction;

            moveAction.action.performed -= AirMoveAction;

            jumpAction.action.started -= JumpAction;
            umbrellaAction.action.started -= UmbrellaAction;
            //stompAction.action.started -= StompAction;
            dashAction.action.started -= DashAction;


            moveAction.action.Disable();
            jumpAction.action.Disable();
            umbrellaAction.action.Disable();
            //stompAction.action.Disable();
            dashAction.action.Enable();
        }

        private void LateUpdate()
        {
            if (!heightSpringBehaviour.GroundedInfo.grounded && uprightSpringBehaviour.LockDirection)
                UmbrellaLookDirection();
        }

        private void UmbrellaLookDirection()
        {
            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

            airUmbrellaBehaviour.LookDir(forward, right);
        }

        private void SwitchDebug(InputAction.CallbackContext context) => OhMyGizmos.Enabled = !OhMyGizmos.Enabled;

        private void MoveAction(InputAction.CallbackContext context)
        {
            Vector2 moveDir = context.ReadValue<Vector2>();

            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up).normalized;
            Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up).normalized;

            moveBehaviour.Move(moveDir.x * right + moveDir.y * forward);
            airUmbrellaBehaviour.Move(moveDir.x * right + moveDir.y * forward);
        }

        private void AirMoveAction(InputAction.CallbackContext context)
        {
            Vector2 moveDir = context.ReadValue<Vector2>();

            Vector3 forward = Vector3.ProjectOnPlane(Camera.main.transform.forward, Vector3.up);
            Vector3 right = Vector3.ProjectOnPlane(Camera.main.transform.right, Vector3.up);

            airBehaviour.Move(moveDir.x * right + moveDir.y * forward);
        }

        private void JumpAction(InputAction.CallbackContext context)
        {
            if(Time.timeScale == 1.0f)
            {
                jumpBehaviour.Jump(animator);
                doubleJumpBehaviour.Jump(animator);
            }
        }

        private void UmbrellaAction(InputAction.CallbackContext context) => airUmbrellaBehaviour.UseUmbrella(animator);

        //private void StompAction(InputAction.CallbackContext context) => stompBehaviour.Stomp(animator);

        private void DashAction(InputAction.CallbackContext context) => dashBehaviour.Dash(animator);
    }
}