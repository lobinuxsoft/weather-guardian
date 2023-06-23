using CryingOnion.OscillatorSystem;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowPath : MonoBehaviour
{
    [SerializeField] private SplineContainer path;
    [SerializeField, Range(0, 1)] private float speed = 0.005f;
    [SerializeField] private AnimationCurve moveBehaviour;

    private Oscillator oscillator;
    private Rigidbody rb;
    private float time = 0;

    private void Start()
    {
        oscillator = GetComponent<Oscillator>();
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        OnValidate();
    }

    private void OnValidate()
    {
        if(oscillator != null)
        {
            time = moveBehaviour.Evaluate(Mathf.PingPong(Time.time * speed, 1));
            path.Evaluate(time, out float3 pos, out float3 tangent, out float3 upVector);
            Vector3 localPos = path.transform.InverseTransformPoint(pos);
            Vector3 forward = (Vector3)tangent;

            rb.MoveRotation(Quaternion.LookRotation(localPos + forward, upVector));

            oscillator.LocalEquilibriumPosition = localPos;
            Vector3 tempPos = Vector3.zero;

            for (int i = 0; i < 3; i++)
            {
                if (oscillator.ForceScale[i] == 0)
                    tempPos[i] = localPos[i];
                else
                    tempPos[i] = transform.position[i];
            }

            rb.MovePosition(tempPos);
        }
    }
}