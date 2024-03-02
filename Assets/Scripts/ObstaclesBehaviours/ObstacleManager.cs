using UnityEngine;
using WeatherGuardian.Utils;

public class ObstacleManager : MonoBehaviour
{
    [Tooltip("Leave it in cero if you want the object to be active forever")]
    [SerializeField] [Range(0.0f, 1000.0f)] float timeObjectWillBeActive = 0.0f;

    [Tooltip("If behaviour can be used more than once if true and only ones if false")]
    [SerializeField] bool loopBehaviour = true;

    [SerializeField] private TriggerCollisionNotifier activationTrigger;

    [SerializeField] private TriggerCollisionNotifier deactivationTrigger;

    private Timer behaviourLifeTime = null;

    private ObstacleBehaviour behaviour = null;

    private void Awake()
    {
        GetScriptsInChildrenObjects();

        if (timeObjectWillBeActive != 0.0f) 
        {
            behaviourLifeTime = new Timer(timeObjectWillBeActive, false);
        }
    }

    private void Start()
    {
        if (activationTrigger != null) 
        {
            activationTrigger.OnTriggerCollisionDetected += behaviour.StartBehaviour;

            activationTrigger.OnTriggerCollisionDetected += StartTimer;

            activationTrigger.OnTriggerCollisionDetected += DeactivateTrigger;
        }

        if (deactivationTrigger != null) 
        {
            deactivationTrigger.OnTriggerCollisionDetected += behaviour.FinishBehaviour;
        }

        behaviour.OnCollision += CollisionFunction;

        if (loopBehaviour && behaviourLifeTime != null) 
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
        if (activationTrigger != null) 
        {
            activationTrigger.OnTriggerCollisionDetected -= behaviour.StartBehaviour;

            activationTrigger.OnTriggerCollisionDetected -= StartTimer;

            activationTrigger.OnTriggerCollisionDetected -= DeactivateTrigger;
        }

        if (deactivationTrigger != null)
        {
            deactivationTrigger.OnTriggerCollisionDetected -= behaviour.FinishBehaviour;
        }

        behaviour.OnCollision -= CollisionFunction;

        if (loopBehaviour && behaviourLifeTime != null)
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
            behaviourLifeTime.UpdateTimerWithResetAndStop();
        }            
    }

    private void GetScriptsInChildrenObjects() 
    {
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
            }
        }
    }

    private void StartTimer() 
    {
        behaviourLifeTime.IsRunning = true;
    }
}