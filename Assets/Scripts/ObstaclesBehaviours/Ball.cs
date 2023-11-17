using System;
using System.IO;
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

    private Quaternion ballInitialRotation;    

    public bool IsActive 
    {
        get 
        {
            return !myRigidBody.isKinematic;
        }
    }    

    void Start()
    {
        ballInitialPosition = transform.localPosition;

        ballInitialRotation = transform.localRotation;

        myRigidBody.isKinematic = true;        
    }

    void Update()
    {
        BallAcceleration();
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

        if (!MyMeshRenderer.isVisible) 
        {
            MyMeshRenderer.enabled = true;
        }

        /*if (!rollingYarnBallSfx.IsNull)
            FMODUnity.RuntimeManager.PlayOneShotAttached(rollingYarnBallSfx, gameObject);*/

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
            Vector3 frontAcceleration = ramp.transform.forward * ballAceleration * Time.deltaTime;

            myRigidBody.velocity += new Vector3(0.0f, 0.0f, frontAcceleration.y);            
        }
    }
}