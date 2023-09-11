using CryingOnion.Timeline.Markers;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine;
using UnityEngine.Timeline;

namespace CryingOnion.Timeline.Editor
{
    [CustomTimelineEditor(typeof(NotificationMarker))]
    public class NotificationMarkerEditor : MarkerEditor
    {
        const float lineOverlayWidth = 2.0f;

        const string overlayPath = "timeline_annotation_overlay";
        const string overlaySelectedPath = "timeline_annotation_overlay_selected";
        const string overlayCollapsedPath = "timeline_annotation_overlay_collapsed";

        static Texture2D overlayTexture;
        static Texture2D overlaySelectedTexture;
        static Texture2D overlayCollapsedTexture;

        static NotificationMarkerEditor()
        {
            overlayTexture = Resources.Load<Texture2D>(overlayPath);
            overlaySelectedTexture = Resources.Load<Texture2D>(overlaySelectedPath);
            overlayCollapsedTexture = Resources.Load<Texture2D>(overlayCollapsedPath);
        }

        // Draws a vertical line on top of the Timeline window's contents.
        public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
        {
            // The `marker argument needs to be cast as the appropriate type, usually the one specified in the `CustomTimelineEditor` attribute
            NotificationMarker notificationMarker = marker as NotificationMarker;

            if (notificationMarker == null) return;

            if (notificationMarker.showLineOverlay)
                DrawLineOverlay(notificationMarker.color, region);

            DrawColorOverlay(region, notificationMarker.color, uiState);
        }

        // Sets the marker's tooltip based on its title.
        public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
        {
            // The `marker argument needs to be cast as the appropriate type, usually the one specified in the `CustomTimelineEditor` attribute
            NotificationMarker notificationMarker = marker as NotificationMarker;

            if (notificationMarker == null)
                return base.GetMarkerOptions(marker);

            if (notificationMarker.notificationSignal == null)
                return new MarkerDrawOptions { tooltip = notificationMarker.name };

            return new MarkerDrawOptions { tooltip = notificationMarker.notificationSignal.name };
        }

        static void DrawLineOverlay(Color color, MarkerOverlayRegion region)
        {
            // Calculate markerRegion's center on the x axis
            float markerRegionCenterX = region.markerRegion.xMin + (region.markerRegion.width - lineOverlayWidth) / 2.0f;

            // Calculate a rectangle that uses the full timeline region's height
            Rect overlayLineRect = new Rect(markerRegionCenterX,
                region.timelineRegion.y,
                lineOverlayWidth,
                region.timelineRegion.height);

            Color overlayLineColor = new Color(color.r, color.g, color.b, color.a * 0.5f);
            EditorGUI.DrawRect(overlayLineRect, overlayLineColor);
        }

        static void DrawColorOverlay(MarkerOverlayRegion region, Color color, MarkerUIStates state)
        {
            // Save the Editor's overlay color before changing it
            Color oldColor = GUI.color;
            GUI.color = color;

            if (state.HasFlag(MarkerUIStates.Selected))
            {
                GUI.DrawTexture(region.markerRegion, overlaySelectedTexture);
            }
            else if (state.HasFlag(MarkerUIStates.Collapsed))
            {
                GUI.DrawTexture(region.markerRegion, overlayCollapsedTexture);
            }
            else if (state.HasFlag(MarkerUIStates.None))
            {
                GUI.DrawTexture(region.markerRegion, overlayTexture);
            }

            // Restore the previous Editor's overlay color
            GUI.color = oldColor;
        }
    }
}