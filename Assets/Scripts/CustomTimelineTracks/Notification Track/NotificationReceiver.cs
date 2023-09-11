using CryingOnion.Timeline.Markers;
using UnityEngine;
using UnityEngine.Playables;

namespace CryingOnion.Timeline.Notification
{
    class NotificationReceiver : MonoBehaviour, INotificationReceiver
    {
        public void OnNotify(Playable origin, INotification notification, object context)
        {
            if (notification != null)
            {
                var marker = (NotificationMarker)notification;
                marker.notificationSignal.EmitSignal();
            }
        }
    }
}