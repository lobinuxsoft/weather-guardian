using CryingOnion.Utils;
using UnityEditor;
using UnityEngine;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// Custom Unity inspector for TorsionalOscillator.
    /// </summary>
    [CustomEditor(typeof(TorsionalOscillator), true)]
    public class TorsionalOscillatorEditor : Editor
    {
        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void RenderCustomGizmo(TorsionalOscillator torcionalOscillator, GizmoType gizmoType)
        {
            Vector3 bob = torcionalOscillator.transform.position;
            Vector3 axis = torcionalOscillator.RotAxis.normalized;
            float angle = torcionalOscillator.angularDisplacementMagnitude;

            Vector3 pivotPosition = torcionalOscillator.transform.TransformPoint(Vector3.Scale(torcionalOscillator.LocalPivotPosition, MathsUtils.Invert(torcionalOscillator.transform.localScale)));
            
            // Draw (wire) pivot position
            Handles.color = Color.white;
            Handles.DrawWireDisc(pivotPosition, -Camera.current.transform.forward, 0.3f);

            // Draw a cross at the pivot position;
            Vector3 cross1 = new Vector3(1, 0, 1) * 0.5f;
            Vector3 cross2 = new Vector3(1, 0, -1) * 0.5f;
            Handles.DrawLine(pivotPosition - cross1, pivotPosition + cross1);
            Handles.DrawLine(pivotPosition - cross2, pivotPosition + cross2);

            // Color goes from green (0,1,0,0) to yellow (1,1,0,0) to red (1,0,0,0).
            Color color = Color.green;
            float upperAmplitude = 90f; // Approximately the upper limit of the angle amplitude within regular use
            color.r = 2f * Mathf.Clamp(angle / upperAmplitude, 0f, 0.5f);
            color.g = 2f * (1f - Mathf.Clamp(angle / upperAmplitude, 0.5f, 1f));

            // Draw (arc) angle to equilibrium
            Handles.color = color;
            
            Vector3 equilibrium = DrawArc(pivotPosition, bob, axis, 0f, -angle / 360f, 32, color);

            // Draw (solid) bob position
            Handles.DrawSolidDisc(bob, -Camera.current.transform.forward, 0.2f);

            Handles.color = Color.white;
            Handles.DrawLine(pivotPosition, equilibrium);

            // Draw (wire) equilibrium position
            Handles.color = Color.green;
            Handles.DrawWireDisc(equilibrium, -Camera.current.transform.forward, 0.3f);
        }

        private static Vector3 DrawArc(Vector3 center, Vector3 point, Vector3 axis, float revFactor1 = 0f, float revFactor2 = 1f, int segments = 48, Color color = default)
        {
            segments = Mathf.Max(1, segments);

            var rad1 = revFactor1 * 2f * Mathf.PI;
            var rad2 = revFactor2 * 2f * Mathf.PI;
            var delta = rad2 - rad1;

            var fsegs = (float)segments;
            var inv_fsegs = 1f / fsegs;

            var vdiff = point - center;
            var length = vdiff.magnitude;
            vdiff.Normalize();

            var prevPoint = point;
            var nextPoint = Vector3.zero;

            if (Mathf.Abs(rad1) >= 1E-6f) prevPoint = PivotAround(center, axis, vdiff, length, rad1);

            for (var seg = 1f; seg <= fsegs; seg++)
            {
                nextPoint = PivotAround(center, axis, vdiff, length, rad1 + delta * seg * inv_fsegs);

                Handles.color = color;
                Handles.DrawLine(prevPoint, nextPoint);
                
                prevPoint = nextPoint;
            }

            return nextPoint;
        }

        private static Vector3 PivotAround(Vector3 center, Vector3 axis, Vector3 dir, float radius, float radians) =>
            center + radius * (Quaternion.AngleAxis(radians * Mathf.Rad2Deg, axis) * dir);
    }
}