using Cinemachine;
using CryingOnion.GizmosRT.Runtime;
using CryingOnion.Timeline.Conditions;
using CryingOnion.Timeline.Notification;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace WeatherGuardian.Gameplay
{
    public class WeatherMachine : MonoBehaviour
    {
        private const string PLAYER_TAG = "Player";
        private const string MACHINE_ANIMATOR_TRACK = "MachineAnimator";
        private const string PLAYER_ANIMATOR_TRACK = "PlayerAnimator";
        private const string PLAYER_TRANSFORM_TRACK = "PlayerTransform";
        private const string CAMERA_TRACK = "MainCamera";

        [Header("Timeline config")]
        [SerializeField] private PlayableDirector director;
        [SerializeField] private Animator machineAnimator;
        [SerializeField] private BoolCondition machineState;
        [SerializeField] private NotificationSignal machineOnNotification;
        [SerializeField] private NotificationSignal machineOffNotification;
        [SerializeField] private TimelineAsset weatherMachineTimeline;

        [Space]
        [Header("Tween Config")]
        [SerializeField] private Transform startPoint;

        [Space]
        [Header("UI Config")]
        [SerializeField] private SpriteRenderer uiButton;

        [Space]
        [Header("Input Config")]
        [SerializeField] private InputActionReference inputAction;

        [Header("Only for debug mode")]
        [SerializeField] private Color debugColor = Color.red;

        private BoxCollider boxTrigger;
        private Transform camTransform;
        private CinemachineBrain cinemachineBrain;
        private GameObject player;

        private Dictionary<string, Object> bindingDic = new Dictionary<string, Object>();

        private void Awake()
        {
            boxTrigger = GetComponent<BoxCollider>();
            boxTrigger.isTrigger = true;

            camTransform = Camera.main.transform;
            cinemachineBrain = Camera.main.gameObject.GetComponent<CinemachineBrain>();

            uiButton.gameObject.SetActive(false);

            inputAction.action.started += OnActionStarted;

            TimelineBinding(ref weatherMachineTimeline, MACHINE_ANIMATOR_TRACK, machineAnimator);
            TimelineBinding(ref weatherMachineTimeline, CAMERA_TRACK, cinemachineBrain);

            machineState.Value = director.playOnAwake;

            director.stopped += OnDirectorStop;

            machineOnNotification.signal += UnbindPlayer;
            machineOffNotification.signal += UnbindPlayer;
        }

        private void OnDestroy()
        {
            director.stopped -= OnDirectorStop;
            machineOnNotification.signal -= UnbindPlayer;
            machineOffNotification.signal -= UnbindPlayer;
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
                uiButton.gameObject.SetActive(false);

                startPoint.position = player.transform.position;
                startPoint.rotation = player.transform.rotation;

                bindingDic.Clear();

                bindingDic.Add(PLAYER_ANIMATOR_TRACK, player != null ? player.GetComponent<Animator>() : null);
                bindingDic.Add(PLAYER_TRANSFORM_TRACK, player != null ? player.transform : null);
                TimelineBinding(ref weatherMachineTimeline, bindingDic);

                machineState.Value = !machineState.Value;

                if (machineState.Value)
                {
                    director.time = 0;
                    director.Play(weatherMachineTimeline, DirectorWrapMode.None);
                }
            }
        }

        private void UnbindPlayer()
        {
            var time = director.time;
            bindingDic.Clear();

            bindingDic.Add(PLAYER_ANIMATOR_TRACK, null);
            bindingDic.Add(PLAYER_TRANSFORM_TRACK, null);
            TimelineBinding(ref weatherMachineTimeline, bindingDic);
            director.RebuildGraph();
            director.time = time;
        }

        private void OnDirectorStop(PlayableDirector director)
        {
            director.time = 0;

            UnbindPlayer();
        }

        private void LateUpdate()
        {
            if (uiButton.isVisible)
            {
                camTransform ??= Camera.main.transform;
                uiButton.transform.rotation = Quaternion.LookRotation(-camTransform.forward);
            }

            if (GizmosRT.Enabled)
                GizmosRT.Cube(Matrix4x4.TRS(boxTrigger.bounds.center, transform.rotation, boxTrigger.size), debugColor);
        }

        private void TimelineBinding(ref TimelineAsset playableAsset, string trackName, Object bindingObj)
        {
            foreach (var output in playableAsset.outputs)
            {
                if (output.streamName.Contains(trackName))
                    director.SetGenericBinding(output.sourceObject, bindingObj);
            }
        }

        private void TimelineBinding(ref TimelineAsset playableAsset, Dictionary<string, Object> bindingDic)
        {
            foreach (var output in playableAsset.outputs)
            {
                if (bindingDic.ContainsKey(output.streamName))
                    director.SetGenericBinding(output.sourceObject, bindingDic[output.streamName]);
            }
        }
    }
}