using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class YarnBallBehaviour : ObstacleBehaviour
{
    [Tooltip("If true when behaviuor ends balls reset position and visibility otherwise it doesnt change")]
    [SerializeField] private bool respawnObject = true;    

    private Rigidbody myRigidBody;

    private Vector3 ballInitialPosition;

    private Quaternion ballInitialRotation;
    
    void Start()
    {
        ballInitialPosition = transform.localPosition;

        ballInitialRotation = transform.localRotation;

        myRigidBody.isKinematic = true;
    }
    

    public override void AwakeConfigs()
    {
        myRigidBody = gameObject.GetComponent<Rigidbody>();
    }

    public override void StartBehaviour()
    {
        myRigidBody.isKinematic = false;

        ResetTransform();
        
        ResetVisibility();
    }

    public override void FinishBehaviour()
    {
        myRigidBody.isKinematic = true;

        myRigidBody.velocity = Vector3.zero;
        
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
        myRigidBody.MovePosition(ballInitialPosition);

        myRigidBody.MoveRotation(ballInitialRotation);
    }
}