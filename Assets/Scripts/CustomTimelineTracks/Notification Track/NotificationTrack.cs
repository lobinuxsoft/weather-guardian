using CryingOnion.Timeline.Markers;
using CryingOnion.Timeline.Notification;
using UnityEngine.Timeline;

namespace CryingOnion.Timeline.Track
{
    [TrackBindingType(typeof(NotificationReceiver))]
    [TrackClipType(typeof(NotificationMarker))]
    public class NotificationTrack : MarkerTrack { }
}