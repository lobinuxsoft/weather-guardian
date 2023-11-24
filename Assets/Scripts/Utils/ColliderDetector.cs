using CryingOnion.Tools.Runtime;
using System;
using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class ColliderDetector : MonoBehaviour
    {
        [SerializeField] private string tagToDetect = "Player";
        [SerializeField] private DetectionType detectionType;

        public event Action<GameObject> onEnter;
        public event Action<GameObject> onStay;
        public event Action<GameObject> onExit;

        [Header("Only for debug mode")]
        [SerializeField] private Color triggerDebugColor = Color.yellow;
        [SerializeField] private Color colliderDebugColor = Color.red;

        private Collider detectCol;

        private void Awake() => detectCol = GetComponent<Collider>();

        private void OnCollisionEnter(Collision collision)
        {
            if ((detectionType & DetectionType.ENTER) == 0) return;

            if (!collision.collider.CompareTag(tagToDetect)) return;

            onEnter?.Invoke(collision.collider.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            if ((detectionType & DetectionType.STAY) == 0) return;

            if (!collision.collider.CompareTag(tagToDetect)) return;

            onStay?.Invoke(collision.collider.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if ((detectionType & DetectionType.EXIT) == 0) return;

            if (!collision.collider.CompareTag(tagToDetect)) return;

            onExit?.Invoke(collision.collider.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((detectionType & DetectionType.ENTER) == 0) return;

            if (!other.CompareTag(tagToDetect)) return;

            onEnter?.Invoke(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if ((detectionType & DetectionType.STAY) == 0) return;

            if (!other.CompareTag(tagToDetect)) return;

            onStay?.Invoke(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((detectionType & DetectionType.EXIT) == 0) return;

            if (!other.CompareTag(tagToDetect)) return;

            onExit?.Invoke(other.gameObject);
        }

        private void LateUpdate()
        {
            if (!OhMyGizmos.Enabled || detectCol == null) return;

            Color color = detectCol.isTrigger ? triggerDebugColor : colliderDebugColor;

            if (detectCol is BoxCollider)
            {
                BoxCollider bc = detectCol as BoxCollider;
                Vector3 scale = new Vector3(bc.size.x * bc.transform.localScale.x, bc.size.y * bc.transform.localScale.y, bc.size.z * bc.transform.localScale.z);
                OhMyGizmos.Cube(Matrix4x4.TRS(bc.bounds.center, bc.transform.rotation, scale), color);
            }
            else if (detectCol is SphereCollider)
            {
                SphereCollider sc = detectCol as SphereCollider;
                float scalar = Mathf.Max(Mathf.Abs(Mathf.Max(sc.transform.localScale.x, sc.transform.localScale.y)), Mathf.Abs(sc.transform.localScale.z));
                OhMyGizmos.Sphere(sc.bounds.center, sc.radius * scalar, color);
            }
        }
    }
}