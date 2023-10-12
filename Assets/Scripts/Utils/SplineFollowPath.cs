using CryingOnion.OscillatorSystem;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SplineFollowPath : MonoBehaviour
{
    [SerializeField] private SplineContainer path;
    [SerializeField, Range(0, 1)] private float speed = 0.005f;
    [SerializeField] private AnimationCurve moveBehaviour;

    public Action OnHalfPath;
    
    private Oscillator oscillator;
    private Rigidbody rb;
    private float time = 0;
    
    //New vars
    private float localTime = 0.0f;

    private bool moving;
    private bool rotate;
    private bool halfPathAchived = false;

    public bool Moving 
    {
        set 
        {
            moving = value;
        }
    }

    public bool Rotate 
    {
        set 
        {
            rotate = value;
        }
    } 

    private void Awake()
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
        if(oscillator != null && moving)
        {
            //Old time code
            //time = moveBehaviour.Evaluate(Mathf.PingPong(Time.time * speed, 1));

            //New time Code
            if (localTime < 1.0f)
            {
                localTime += Time.deltaTime * speed;

                if (localTime >= 0.5f) 
                {
                    if (!halfPathAchived) 
                    {
                        OnHalfPath?.Invoke();

                        halfPathAchived = true;
                    }
                }                
            }
            else 
            {
                localTime = 0.0f;

                halfPathAchived = false;
            }

            time = moveBehaviour.Evaluate(Mathf.PingPong(localTime, 1));

            CalculatePosition();
        }
    }

    public void ResetPath() 
    {
        moving = false;

        localTime = 0.0f;

        time = 0.0f;

        CalculatePosition();
    }

    private void CalculatePosition() 
    {
        path.Evaluate(time, out float3 pos, out float3 tangent, out float3 upVector);
        Vector3 localPos = path.transform.InverseTransformPoint(pos);
        Vector3 forward = (Vector3)tangent;

        if (rotate)
        {
            rb.MoveRotation(Quaternion.LookRotation(localPos + forward, upVector));
        }

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