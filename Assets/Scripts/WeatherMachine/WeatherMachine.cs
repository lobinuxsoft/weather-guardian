using CryingOnion.OhMy.WeatherSystem.Core;
using CryingOnion.OhMy.WeatherSystem.Data;
using CryingOnion.Tools.Runtime;
using FMODUnity;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.VFX;
using WeatherGuardian.Behaviours;
using WeatherGuardian.PickUps;

namespace WeatherGuardian.Gameplay
{
    public class WeatherMachine : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";
        private const float cameraChangeDuration = 2.5f;
        private const float playerInactiveDuration = 2.5f;
        private const float machineChangeStateDelay = 1.0f;

        [Tooltip("The machine ON/OFF state on game start.")]
        [SerializeField] private bool machineActiveOnStart = true;

        [Space]
        [Header("Animator Config")]
        [SerializeField] private Animator machineAnimator;

        [Space]
        [Header("Camera Config")]
        [SerializeField] private GameObject virtualCamera;

        [Space]
        [Header("Player Position Config")]
        [SerializeField] private Transform playerInteractionTransform;
        [SerializeField] private AnimationCurve lerpBehaviour;

        [Space]
        [Header("VFX Config")]
        [SerializeField] private VisualEffect smokeVfx;

        [Space]
        [Header("FMOD Config")]
        [SerializeField] private StudioEventEmitter startSfxEvent;
        [SerializeField] private StudioEventEmitter loopSfxEvent;
        [SerializeField] private StudioEventEmitter stopSfxEvent;

        [Space]
        [Header("UI Config")]
        [SerializeField] private SpriteRenderer uiButton;

        [Space]
        [Header("Input Config")]
        [SerializeField] private InputActionReference inputAction;

        [Space]
        [Header("Weather Config")]
        [SerializeField] private OMWSWeatherProfile weatherProfile;
        [SerializeField, Range(0, 15)] private float trasitionDuration = 2.5f;

        [Space]
        [Header("Machine State Event Config")]
        [Space]
        [SerializeField] private UnityEvent onMachineOn;
        [Space]
        [SerializeField] private UnityEvent onMachineOff;


        [Space]
        [Header("Only for debug mode")]
        [SerializeField] private Color debugColor = Color.red;

        [Space]
        [Header("Collectable Settings")]
        [SerializeField] private ItemCollector collections;

        private int machineStateHash = Animator.StringToHash("STATE");
        private int playerInteractHash = Animator.StringToHash("INTERACT");

        private BoxCollider boxTrigger;
        private Transform camTransform;
        private GameObject player;
        private bool machineIsOn = false;

        public static System.EventHandler OnMachineOff;

        private void Awake()
        {
            boxTrigger = GetComponent<BoxCollider>();
            boxTrigger.isTrigger = true;

            camTransform = Camera.main.transform;

            uiButton.gameObject.SetActive(false);

            inputAction.action.started += OnActionStarted;
        }

        private void Start() => StartCoroutine(MachineInitState(machineActiveOnStart));

        private IEnumerator MachineInitState(bool state)
        {
            yield return new WaitForEndOfFrame();

            machineAnimator.SetBool(machineStateHash, state);

            if (state)
            {
                loopSfxEvent.Play();
                smokeVfx.Play();
                onMachineOn?.Invoke();

                OMWSWeather.instance.weatherSelectionMode = OMWSEcosystem.EcosystemStyle.manual;
                OMWSWeather.instance.SetWeather(weatherProfile, trasitionDuration);
            }
            else
            {
                loopSfxEvent.Stop();
                smokeVfx.Stop();
                onMachineOff?.Invoke();
            }
        }

        private void LateUpdate()
        {
            if (uiButton.isVisible)
            {
                camTransform ??= Camera.main.transform;
                uiButton.transform.rotation = Quaternion.LookRotation(-camTransform.forward);
            }

            if (OhMyGizmos.Enabled)
                OhMyGizmos.Cube(Matrix4x4.TRS(boxTrigger.bounds.center, transform.rotation, boxTrigger.size), debugColor);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                uiButton.gameObject.SetActive(true);
                player = other.gameObject;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(PLAYER_TAG))
            {
                uiButton.gameObject.SetActive(false);
                player = null;
            }
        }

        private void OnActionStarted(InputAction.CallbackContext context)
        {
            if (uiButton.gameObject.activeSelf)
            {   
                if (collections==null || collections.HasAllMachineParts())
                {
                    if (!machineIsOn)
                    {
                        machineIsOn = true;
                        activeMachine();
                        OnMachineOff?.Invoke(this, EventArgs.Empty);
                    }
                }

            }
        }

        private void activeMachine()
        {
            Debug.Log("Machine active");
            uiButton.gameObject.SetActive(false);
            UpdateMachineState(!machineAnimator.GetBool(machineStateHash));
        }

        public void UpdateMachineState(bool value)
        {
            StartCoroutine(CameraChangeRoutine(cameraChangeDuration));
            StartCoroutine(PlayerInactiveRoutine(playerInactiveDuration));
            StartCoroutine(MachineStateRoutine(value, machineChangeStateDelay));
        }

        private IEnumerator CameraChangeRoutine(float duration)
        {
            virtualCamera.gameObject.SetActive(true);
            yield return new WaitForSeconds(duration);
            virtualCamera.gameObject.SetActive(false);
        }

        private IEnumerator PlayerInactiveRoutine(float duration)
        {
            float lerp = 0;
            Animator playerAnim = player.GetComponent<Animator>();
            MoveBehaviour mb = playerAnim.GetBehaviour<MoveBehaviour>();
            HeightSpringBehaviour hsb = playerAnim.GetBehaviour<HeightSpringBehaviour>();

            Vector3 startPos = hsb.Body.position;
            Quaternion startRot = hsb.Body.rotation;

            playerAnim.SetBool(playerInteractHash, true);
            mb.CanMove = false;
            hsb.Body.useGravity = false;

            while (lerp < duration)
            {
                lerp += Time.deltaTime;

                hsb.Body.position = Vector3.Lerp(startPos, playerInteractionTransform.position, lerpBehaviour.Evaluate(lerp / duration));
                hsb.Body.rotation = Quaternion.Lerp(startRot, playerInteractionTransform.rotation, lerpBehaviour.Evaluate(lerp / duration));

                yield return null;
            }

            hsb.Body.position = playerInteractionTransform.position;
            hsb.Body.rotation = playerInteractionTransform.rotation;
            hsb.Body.useGravity = true;

            playerAnim.SetBool(playerInteractHash, false);
            mb.CanMove = true;
        }

        private IEnumerator MachineStateRoutine(bool state, float delay)
        {
            yield return new WaitForSeconds(delay);
            machineAnimator.SetBool(machineStateHash, state);

            if (state)
            {
                startSfxEvent.Play();
                loopSfxEvent.Play();
                smokeVfx.Play();

                OMWSWeather.instance.weatherSelectionMode = OMWSEcosystem.EcosystemStyle.manual;
                OMWSWeather.instance.SetWeather(weatherProfile, trasitionDuration);
                onMachineOn?.Invoke();
            }
            else
            {
                loopSfxEvent.Stop();
                stopSfxEvent.Play();
                smokeVfx.Stop();

                onMachineOff?.Invoke();
                OMWSWeather.instance.weatherSelectionMode = OMWSEcosystem.EcosystemStyle.forecast;
                OMWSWeather.instance.SetWeather(
                    OMWSWeather.instance.forecastProfile.initialProfile,
                    trasitionDuration);
            }
        }

        public void EmitSmokeVFX() => smokeVfx.Reinit();
    }
}