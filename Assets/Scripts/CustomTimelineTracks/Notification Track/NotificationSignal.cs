using System;
using UnityEngine;

namespace CryingOnion.Timeline.Notification
{
    [CreateAssetMenu(fileName = nameof(NotificationSignal), menuName = "CryingOnion/Timeline/Notification/NotificationSignal")]
    public class NotificationSignal : ScriptableObject
    {
        public event Action signal;

        public void EmitSignal() => signal?.Invoke();
    }
}