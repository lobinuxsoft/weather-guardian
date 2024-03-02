using UnityEngine;
using UnityEngine.Events;

namespace CryingOnion.Timeline.Notification
{
    public class NotificationSubscriptor : MonoBehaviour
    {
        [SerializeField] NotificationSignal notificationSignal;

        public UnityEvent onSignalEmit;

        private void Awake() => notificationSignal.signal += OnSignalEmit;

        private void OnDestroy() => notificationSignal.signal -= OnSignalEmit;

        private void OnSignalEmit() => onSignalEmit.Invoke();
    }
}