using System;
using System.Drawing.Text;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Ball : ObstacleBehaviour
{
    [Tooltip("If true when behaviuor ends balls reset position and visibility otherwise it doesnt change")]
    [SerializeField] private bool respawnObject = true;

    [SerializeField] [Range(0.01f, 30.0f)] private float ballAceleration;

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
        myRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public override void StartBehaviour()
    {
        myRigidBody.isKinematic = false;

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
    }

    public override void CollisionBehaviour()
    {
        FinishBehaviour();
    }

    public void ResetTransform() 
    {
        transform.localPosition = ballInitialPosition;
        
        transform.localRotation = ballInitialRotation;
    }

    /// <summary>
    /// Accelerates the ball in the front direction
    /// </summary>        
    /// <returns>The oscillation force</returns>

    private void BallAcceleration() 
    {
        if (!myRigidBody.isKinematic)
        {
            myRigidBody.velocity += new Vector3(0.0f, -1.0f, 1.0f) * ballAceleration * Time.deltaTime;
        }
    }
}