using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class YarnBallBehaviour : ObstacleBehaviour
{
    [Tooltip("If true when behaviuor ends balls reset position and visibility otherwise it doesnt change")]
    [SerializeField] private bool respawnObject = true;

    [SerializeField] [Range(0.01f, 30.0f)] private float ballAceleration;

    private Rigidbody myRigidBody;

    private Vector3 ballInitialPosition;

    private Quaternion ballInitialRotation;
    
    void Start()
    {
        ballInitialPosition = transform.localPosition;

        ballInitialRotation = transform.localRotation;

        myRigidBody.isKinematic = true;
    }

    void Update()
    {
        if (!myRigidBody.isKinematic) 
        {
            myRigidBody.velocity += new Vector3(0.0f, -1.0f, 1.0f) * ballAceleration * Time.deltaTime;
        }
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
}