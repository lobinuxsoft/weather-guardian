using UnityEngine;

namespace WeatherGuardian.Utils
{
    [RequireComponent(typeof(ColliderDetector))]
    public class CheckPoint : MonoBehaviour
    {
        public static Vector3? LastPos { get; private set; }
        public static Quaternion? LastRot { get; private set; }

        private ColliderDetector colliderDetector;

        private void Awake()
        {
            colliderDetector = GetComponent<ColliderDetector>();

            colliderDetector.onEnter += UpdateCheckPoint;
            colliderDetector.onStay += UpdateCheckPoint;
            colliderDetector.onExit += UpdateCheckPoint;

            if (LastPos == null && LastRot == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                LastPos ??= player != null ? player.transform.position : Vector3.zero;
                LastRot ??= player != null ? player.transform.rotation : Quaternion.identity;
            }
        }

        private void OnDestroy()
        {
            if (colliderDetector != null)
            {
                colliderDetector.onEnter -= UpdateCheckPoint;
                colliderDetector.onStay -= UpdateCheckPoint;
                colliderDetector.onExit -= UpdateCheckPoint;
            }
        }

        private void UpdateCheckPoint(GameObject go)
        {
            LastPos = transform.position;
            LastRot = transform.rotation;
        }

        public static void JumpToLastCheckPoint(GameObject go)
        {
            go.transform.position = LastPos ?? Vector3.zero;
            go.transform.rotation = LastRot ?? Quaternion.identity;
        }
    }
}