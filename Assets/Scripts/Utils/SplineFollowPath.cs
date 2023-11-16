using CryingOnion.OscillatorSystem;
using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using WeatherGuardian.Utils;

public class SplineFollowPath : MonoBehaviour
{
    [SerializeField] private SplineContainer path;
    [SerializeField, Range(0, 1)] private float speed = 0.005f;
    [SerializeField] private AnimationCurve moveBehaviour;
    [SerializeField, Range(0.0f, 20.0f)] private float startDelay = 1.0f;

    public event Action OnHalfPath;
    public event Action OnEnd;

    private Oscillator oscillator;
    private Rigidbody rb;

    private Timer timerToStartMoving = null;
    //private float time = 0;

    //New vars
    private float localTime;
    private float time;

    private bool moving;
    private bool rotate;
    private bool halfPathAchived;    

    public bool Moving
    {
        set
        {
            moving = value;            
        }
        get 
        {
            return moving;
        }
    }

    public bool Rotate
    {
        set
        {
            rotate = value;
        }
    }

    public bool DelayFinished
    {        
        get
        {
            return startDelay <= 0.0f;
        }
    }

    private void Awake()
    {
        oscillator = GetComponent<Oscillator>();
        rb = GetComponent<Rigidbody>();

        if (startDelay > 0.0f)
            timerToStartMoving = new Timer(startDelay);
    }

    private void Start()
    {
        if (timerToStartMoving == null) 
        {
            moving = true;
        }
        else 
        {
            moving = false;

            timerToStartMoving.OnTimerEnds += SetMovingToTrue;
        }

        halfPathAchived = false;

        time = 0.0f;

        localTime = 0.0f;
    }

    private void Update()
    {
        if (timerToStartMoving != null)
            timerToStartMoving.UpdateTimer();        
    }

    private void FixedUpdate()
    {
        OnValidate();                
    }

    private void OnValidate()
    {
        if (oscillator != null && moving)
        {
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

                OnEnd?.Invoke();
            }

            time = moveBehaviour.Evaluate(Mathf.PingPong(localTime, 1));

            CalculatePosition();
        }
    }

    private void OnDestroy()
    {
        if(timerToStartMoving != null)
            timerToStartMoving.OnTimerEnds -= SetMovingToTrue;
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
            rb.MoveRotation(Quaternion.LookRotation(localPos + forward, upVector));

        oscillator.EquilibriumPosition = pos;
    }

    private void SetMovingToTrue() 
    {
        moving = true;
    }
}