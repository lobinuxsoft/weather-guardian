using UnityEngine;

namespace WeatherGuardian.Utils
{
    [RequireComponent(typeof(ColliderDetector))]
    public class KillerComponent : MonoBehaviour
    {
        private ColliderDetector colliderDetector;

        private void Awake()
        {
            colliderDetector = GetComponent<ColliderDetector>();

            colliderDetector.onEnter += Kill;
            colliderDetector.onStay += Kill;
            colliderDetector.onExit += Kill;
        }

        private void OnDestroy()
        {
            if (colliderDetector != null)
            {
                colliderDetector.onEnter -= Kill;
                colliderDetector.onStay -= Kill;
                colliderDetector.onExit -= Kill;
            }
        }

        private void Kill(GameObject go) => CheckPoint.JumpToLastCheckPoint(go);
    }
}