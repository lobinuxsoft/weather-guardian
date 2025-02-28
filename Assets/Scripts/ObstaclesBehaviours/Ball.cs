using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : ObstacleBehaviour
{
    [SerializeField] private GameObject ramp;

    [Tooltip("If true when behaviuor ends balls reset position and visibility otherwise it doesnt change")]
    [SerializeField] private bool respawnObject = true;

    [SerializeField] [Range(0.01f, 30.0f)] private float ballAceleration;

    [SerializeField] private FMODUnity.EventReference stopYarnBallRollingEvent;

    //[SerializeField] private FMODUnity.EventReference rollingYarnBallSfx;

    [SerializeField] private FMODUnity.EventReference yarnBallImpactSfx;

    private Rigidbody myRigidBody;

    private Vector3 ballInitialPosition;

    private Vector3 acceleration;

    private Vector3 accelerationDirection;

    private Quaternion ballInitialRotation;

    private float actualTime = 0;

    [SerializeField] private float timeToStart = 0;

    private bool started = false;

    public bool IsActive 
    {
        get 
        {
            return !myRigidBody.isKinematic;
        }
    }    

    void Start()
    {
        SetStartConfigurations();
        //timeToStart is grater than 0 if the game designer wants to delay the ball behaviour to start
        if (timeToStart > 0)
        {
            myRigidBody.useGravity = false;
        }
        
    }

    void Update()
    {
        if (actualTime < timeToStart)
        {
            actualTime += Time.deltaTime;
            
        }
        else
        {
            if (!started)
            {
                started = true;
                myRigidBody.useGravity = true;
            }
            BallAcceleration();
        }

        
    }

    private void SetStartConfigurations()
    {
        ballInitialPosition = transform.localPosition;

        ballInitialRotation = ramp.transform.localRotation;

        transform.localRotation = ballInitialRotation;

        accelerationDirection = ramp.transform.forward;

        acceleration = accelerationDirection * ballAceleration;

        myRigidBody.isKinematic = true;
    }

    public override void AwakeConfigs()
    {
        if (ramp == null)
        {
            Debug.LogError("No ramp assigned in ramp GameObject!");
        }
        else 
        {
            transform.localRotation = ramp.transform.localRotation;
        }

        myRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public override void StartBehaviour()
    {
        myRigidBody.isKinematic = false;        

        myRigidBody.linearVelocity = Vector3.zero;

        if (!MyMeshRenderer.isVisible) 
        {
            MyMeshRenderer.enabled = true;
        }        

        ResetTransform();               
    }

    public override void FinishBehaviour()
    {
        myRigidBody.isKinematic = true;
        
        if (respawnObject) 
        {
            ResetTransform();

            ResetVisibility();
        }

        if (!stopYarnBallRollingEvent.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(stopYarnBallRollingEvent, gameObject);
    }

    public override void CollisionBehaviour()
    {
        if (!yarnBallImpactSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShot(yarnBallImpactSfx);

        myRigidBody.linearVelocity = Vector3.zero;
    }

    public void ResetTransform() 
    {
        transform.localPosition = ballInitialPosition;
        
        transform.localRotation = ballInitialRotation;
    }

    /// <summary>
    /// Accelerates the ball in the front direction in regarding of the plataform that contins it
    /// </summary>        
    /// <returns>The oscillation force</returns>

    private void BallAcceleration() 
    {
        if (!myRigidBody.isKinematic && ramp != null)
        {
            myRigidBody.linearVelocity += acceleration * Time.deltaTime;            
        }
    }
}