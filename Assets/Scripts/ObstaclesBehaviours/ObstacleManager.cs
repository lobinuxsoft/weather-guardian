using UnityEngine;
using WeatherGuardian.Utils;

public class ObstacleManager : MonoBehaviour
{
    [Tooltip("Leave it in cero if you want the object to be active forever")]
    [SerializeField] [Range(0.0f, 1000.0f)] float timeObjectWillBeActive = 0.0f;

    [Tooltip("If behaviour can be used more than once if true and only ones if false")]
    [SerializeField] bool loopBehaviour = true;

    private Timer behaviourLifeTime = null;

    private ObstacleBehaviour behaviour = null;

    private ObstacleActivationTrigger activationTrigger = null;

    private void Awake()
    {
        GetScriptsInChildrenObjects();

        if (timeObjectWillBeActive != 0.0f) 
        {
            behaviourLifeTime = new Timer(timeObjectWillBeActive);
        }
    }

    private void Start()
    {
        activationTrigger.OnTriggerCollisionDetected += behaviour.StartBehaviour;

        activationTrigger.OnTriggerCollisionDetected += DeactivateTrigger;

        behaviour.OnCollision += CollisionFunction;

        if (loopBehaviour) 
        {
            behaviourLifeTime.OnTimerEnds += ActivateTrigger;
        }

        if (behaviourLifeTime != null) 
        {
            behaviourLifeTime.OnTimerEnds += behaviour.FinishBehaviour;            
        }
    }

    private void OnDestroy()
    {
        activationTrigger.OnTriggerCollisionDetected -= behaviour.StartBehaviour;

        behaviour.OnCollision -= CollisionFunction;

        if (loopBehaviour)
        {
            behaviourLifeTime.OnTimerEnds -= ActivateTrigger;
        }

        if (behaviourLifeTime != null)
        {
            behaviourLifeTime.OnTimerEnds -= behaviour.FinishBehaviour;
        }
    }

    private void Update()
    {
        if (behaviourLifeTime != null) 
        {            
            behaviourLifeTime.UpdateTimerWithReset();
        }            
    }

    private void GetScriptsInChildrenObjects() 
    {
        activationTrigger = gameObject.GetComponentInChildren<ObstacleActivationTrigger>(true);

        if (activationTrigger == null)
            Debug.LogError("There is no ObstacleActivationTrigger script in any child object!");

        behaviour = gameObject.GetComponentInChildren<ObstacleBehaviour>(true);

        if(behaviour == null)
            Debug.LogError("There is no ObstacleActivationBehaviour script in any child object!");
    }

    private void ActivateTrigger() 
    {
        if (activationTrigger != null) 
        {
            activationTrigger.Active = true;
        }
    }

    private void DeactivateTrigger() 
    {
        if (activationTrigger != null) 
        {
            activationTrigger.Active = false;
        }
    }

    private void CollisionFunction()
    {
        for (short i = 0; i < behaviour.Tags.Length; i++)
        {            
            if (behaviour.Tags[i] == behaviour.LastObjectToCollideWith.transform.tag) 
            {
                behaviour.CollisionBehaviour();

                behaviourLifeTime.ResetTimer();

                ActivateTrigger();
            }
        }
    }
}