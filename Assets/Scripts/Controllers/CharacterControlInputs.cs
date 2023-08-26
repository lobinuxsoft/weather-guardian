using CryingOnion.OscillatorSystem;
using WeatherGuardian.Behaviours;
using UnityEngine;
using UnityEngine.InputSystem;
using CryingOnion.GizmosRT.Runtime;

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
        [SerializeField] InputActionReference stompAction;
        [SerializeField] InputActionReference dashAction;

        [SerializeField] Animator animator;
        [SerializeField] Oscillator oscillator;

        private HeightSpringBehaviour heightSpringBehaviour;
        private MoveBehaviour moveBehaviour;
        private JumpBehaviour jumpBehaviour;
        private DoubleJumpBehaviour doubleJumpBehaviour;
        private AirBehaviour airBehaviour;
        private AirUmbrellaBehaviour airUmbrellaBehaviour;
        private StompBehaviour stompBehaviour;
        private DashBehaviour dashBehaviour;

        private void Awake()
        {
            debugAction.action.started += SwitchDebug;
            debugAction.action.Enable();

            moveAction.action.performed += MoveAction;
            moveAction.action.canceled += MoveAction;

            moveAction.action.performed += AirMoveAction;

            jumpAction.action.started += JumpAction;
            umbrellaAction.action.started += UmbrellaAction;
            stompAction.action.started += StompAction;
            dashAction.action.started += DashAction;

            moveAction.action.Enable();
            jumpAction.action.Enable();
            umbrellaAction.action.Enable();
            stompAction.action.Enable();
            dashAction.action.Enable();
        }

        private void Start()
        {
            heightSpringBehaviour = animator.GetBehaviour<HeightSpringBehaviour>();
            heightSpringBehaviour.Oscillator = oscillator;

            moveBehaviour = animator.GetBehaviour<MoveBehaviour>();
            jumpBehaviour = animator.GetBehaviour<JumpBehaviour>();
            doubleJumpBehaviour = animator.GetBehaviour<DoubleJumpBehaviour>();
            airBehaviour = animator.GetBehaviour<AirBehaviour>();
            airUmbrellaBehaviour = animator.GetBehaviour<AirUmbrellaBehaviour>();
            stompBehaviour = animator.GetBehaviour<StompBehaviour>();
            dashBehaviour = animator.GetBehaviour<DashBehaviour>();
        }

        private void OnDestroy()
        {
            debugAction.action.started -= SwitchDebug;
            debugAction.action.Disable();

            moveAction.action.performed -= MoveAction;
            moveAction.action.canceled -= MoveAction;

            moveAction.action.performed -= AirMoveAction;

            jumpAction.action.started -= JumpAction;
            umbrellaAction.action.started -= UmbrellaAction;
            stompAction.action.started -= StompAction;
            dashAction.action.started -= DashAction;


            moveAction.action.Disable();
            jumpAction.action.Disable();
            umbrellaAction.action.Disable();
            stompAction.action.Disable();
            dashAction.action.Enable();
        }

        private void SwitchDebug(InputAction.CallbackContext context) => GizmosRT.Enabled = !GizmosRT.Enabled;

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
            jumpBehaviour.Jump(animator);
            doubleJumpBehaviour.Jump(animator);
        }

        private void UmbrellaAction(InputAction.CallbackContext context) => airUmbrellaBehaviour.UseUmbrella(animator);

        private void StompAction(InputAction.CallbackContext context) => stompBehaviour.Stomp(animator);

        private void DashAction(InputAction.CallbackContext context) => dashBehaviour.Dash(animator);
    }
}