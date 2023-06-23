using UnityEngine;
using UnityEditor;

namespace CryingOnion.OscillatorSystem
{
    /// <summary>
    /// Custom Unity inspector for Oscillator.
    /// </summary>
    [CustomEditor(typeof(Oscillator), true)]
    public class OscillatorEditor : Editor
    {
        /// <summary>
        /// Draw the default inspector, with a clamped Vector3 on the forceScale.
        /// </summary>
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            Oscillator oscillator = (Oscillator)target;
            Vector3 forceScale = oscillator.ForceScale;

            for (int i = 0; i < 3; i++)
            {
                forceScale[i] = (int)Mathf.Clamp01(oscillator.ForceScale[i]);
            }

            oscillator.ForceScale = forceScale;
        }

        [DrawGizmo(GizmoType.Selected | GizmoType.NonSelected)]
        static void RenderCustomGizmo(Oscillator oscillator, GizmoType gizmoType)
        {
            Vector3 bob = oscillator.transform.localPosition;
            Vector3 equilibrium = oscillator.LocalEquilibriumPosition;
            if (oscillator.transform.parent != null)
            {
                bob += oscillator.transform.parent.position;
                equilibrium += oscillator.transform.parent.position;
            }

            /* Draw (wire) equilibrium position */
            Color color = Color.green;
            Handles.color = color;
            Handles.DrawWireDisc(equilibrium, -Camera.current.transform.forward, 0.3f);

            /* Draw (solid) bob position. */
            /* Color goes from green (0,1,0,0) to yellow (1,1,0,0) to red (1,0,0,0). */
            /* Approximately the upper limit of the amplitude within regular use. */
            float upperAmplitude = oscillator.Stiffness * oscillator.Mass / (3f * 100f);
            color.r = 2f * Mathf.Clamp(Vector3.Magnitude(bob - equilibrium) * upperAmplitude, 0f, 0.5f);
            color.g = 2f * (1f - Mathf.Clamp(Vector3.Magnitude(bob - equilibrium) * upperAmplitude, 0.5f, 1f));
            color.a = 0.5f;

            Handles.color = color;
            Handles.DrawSolidDisc(bob, -Camera.current.transform.forward, 0.2f);
            Handles.DrawLine(bob, equilibrium);
        }
    }
}