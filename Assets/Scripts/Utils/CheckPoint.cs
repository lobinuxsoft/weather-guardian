using CryingOnion.Tools.Runtime;
using UnityEngine;

namespace WeatherGuardian.Utils
{
    [RequireComponent(typeof(BoxCollider))]
    public class CheckPoint : MonoBehaviour
    {
        public static Vector3? LastPos { get; private set; }
        public static Quaternion? LastRot { get; private set; }

        [SerializeField] private DetectionType detectionType;

        private BoxCollider trigerCollider;

        private void Awake()
        {
            trigerCollider = GetComponent<BoxCollider>();
            trigerCollider.isTrigger = true;

            if (LastPos == null && LastRot == null)
            {
                var player = GameObject.FindGameObjectWithTag("Player");
                LastPos ??= player != null ? player.transform.position : Vector3.zero;
                LastRot ??= player != null ? player.transform.rotation : Quaternion.identity;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((detectionType & DetectionType.ENTER) == 0) return;

            LastPos = transform.position;
            LastRot = transform.rotation;
        }

        private void OnTriggerStay(Collider other)
        {
            if ((detectionType & DetectionType.STAY) == 0) return;

            LastPos = transform.position;
            LastRot = transform.rotation;
        }

        private void OnTriggerExit(Collider other)
        {
            if ((detectionType & DetectionType.EXIT) == 0) return;

            LastPos = transform.position;
            LastRot = transform.rotation;
        }

        public static void JumpToLastCheckPoint(GameObject go)
        {
            go.transform.position = LastPos ?? Vector3.zero;
            go.transform.rotation = LastRot ?? Quaternion.identity;
        }

        private void LateUpdate() => OhMyGizmos.Cube(transform.localToWorldMatrix, Color.green);
    }
}