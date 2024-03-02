using CryingOnion.Timeline.Notification;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryingOnion.Timeline.Markers
{
    [CustomStyle(nameof(NotificationMarker))]
    [DisplayName(nameof(NotificationMarker))]
    public class NotificationMarker : Marker, INotification, INotificationOptionProvider
    {
        public Color color = new Color(1.0f, 1.0f, 1.0f, 0.5f);
        public bool showLineOverlay = true;
        public bool emitOnce;
        public bool emitInEditor = true;
        public NotificationSignal notificationSignal;

        public PropertyName id { get; }

        public NotificationFlags flags =>
            (emitOnce ? NotificationFlags.TriggerOnce : default) | (emitInEditor ? NotificationFlags.TriggerInEditMode : default);
    }
}