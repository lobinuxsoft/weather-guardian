using UnityEngine;
using WeatherGuardian.Behaviours;

namespace WeatherGuardian.Utils
{
    [RequireComponent(typeof(ColliderDetector))]
    public class PushPlayerComponent : MonoBehaviour
    {
        [SerializeField] private float pushForce = 20;

        private ColliderDetector colliderDetector;
        private int umbrellaHash = Animator.StringToHash("UMBRELLA");
        private Rigidbody rb;

        private void Awake()
        {
            rb = transform.GetComponentInParent<Rigidbody>();
            colliderDetector = GetComponent<ColliderDetector>();
            colliderDetector.onEnter += Push;
            colliderDetector.onStay += Push;
            colliderDetector.onExit += Push;
        }

        private void OnDestroy()
        {
            if (colliderDetector != null)
            {
                colliderDetector.onEnter -= Push;
                colliderDetector.onStay -= Push;
                colliderDetector.onExit -= Push;
            }
        }

        private void Push(GameObject go)
        {
            Animator animator = go.GetComponent<Animator>();
            HeightSpringBehaviour hsb = animator.GetBehaviour<HeightSpringBehaviour>();

            float dotVel = Vector3.Dot(rb.linearVelocity, transform.forward);

            if(dotVel > 0)
            {
                animator.SetBool(umbrellaHash, false);
                hsb.Body.AddForce(rb.linearVelocity, ForceMode.Impulse);
            }
        }
    }
}