using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public abstract class ObstacleBehaviour : MonoBehaviour
{
    [Tooltip("Whethear you want the obstacle shape to be visible at start or not")]
    [SerializeField] private bool startsVisible;

    [Tooltip("Tags of objects that triggers colision behaviour of Yarn Ball")]
    [SerializeField] private string[] tagsForCollisionDetection;

    private MeshRenderer myMeshRenderer;

    public event Action OnCollision;

    private Collision lastObjectToCollideWith = null;

    public Collision LastObjectToCollideWith 
    {
        get 
        {
            return lastObjectToCollideWith;
        }
    }

    public string[] Tags 
    {
        get 
        {
            return tagsForCollisionDetection;
        }
    }

    public MeshRenderer MyMeshRenderer
    {
        set 
        {
            myMeshRenderer = value;
        }
        get 
        {
            return myMeshRenderer;
        }
    }

    private void Awake()
    {
        myMeshRenderer = GetComponent<MeshRenderer>();

        myMeshRenderer.enabled = startsVisible;

        AwakeConfigs();
    }

    private void OnCollisionEnter(Collision collision)
    {
        lastObjectToCollideWith = collision;

        OnCollision?.Invoke();
    }

    public void ResetVisibility() 
    {
        myMeshRenderer.enabled = startsVisible;
    }    

    public abstract void AwakeConfigs();

    public abstract void StartBehaviour();

    public abstract void FinishBehaviour();

    public abstract void CollisionBehaviour();
}