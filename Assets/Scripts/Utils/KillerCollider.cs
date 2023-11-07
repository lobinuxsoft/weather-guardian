using CryingOnion.Tools.Runtime;
using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class KillerCollider : MonoBehaviour
    {
        [SerializeField] private string tagToCollide = "Player";
        [SerializeField] private DetectionType detectionType;

        [Header("Only for debug mode")]
        [SerializeField] private Color triggerDebugColor = Color.yellow;
        [SerializeField] private Color colliderDebugColor = Color.red;

        private Collider killerCol;

        private void Awake() => killerCol = GetComponent<Collider>();

        private void OnCollisionEnter(Collision collision)
        {
            if ((detectionType & DetectionType.ENTER) == 0) return;

            if (!collision.collider.CompareTag(tagToCollide)) return;

            CheckPoint.JumpToLastCheckPoint(collision.gameObject);
        }

        private void OnCollisionStay(Collision collision)
        {
            if ((detectionType & DetectionType.STAY) == 0) return;

            if (!collision.collider.CompareTag(tagToCollide)) return;

            CheckPoint.JumpToLastCheckPoint(collision.gameObject);
        }

        private void OnCollisionExit(Collision collision)
        {
            if ((detectionType & DetectionType.EXIT) == 0) return;

            if (!collision.collider.CompareTag(tagToCollide)) return;

            CheckPoint.JumpToLastCheckPoint(collision.gameObject);
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((detectionType & DetectionType.ENTER) == 0) return;

            if (!other.CompareTag(tagToCollide)) return;

            CheckPoint.JumpToLastCheckPoint(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if ((detectionType & DetectionType.STAY) == 0) return;

            if (!other.CompareTag(tagToCollide)) return;

            CheckPoint.JumpToLastCheckPoint(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((detectionType & DetectionType.EXIT) == 0) return;

            if (!other.CompareTag(tagToCollide)) return;

            CheckPoint.JumpToLastCheckPoint(other.gameObject);
        }

        private void LateUpdate()
        {
            if (!OhMyGizmos.Enabled) return;

            Color color = killerCol.isTrigger ? triggerDebugColor : colliderDebugColor;

            if (killerCol is BoxCollider)
            {
                BoxCollider bc = killerCol as BoxCollider;
                Vector3 scale = new Vector3(bc.size.x * bc.transform.localScale.x, bc.size.y * bc.transform.localScale.y, bc.size.z * bc.transform.localScale.z);
                OhMyGizmos.Cube(Matrix4x4.TRS(bc.bounds.center, bc.transform.rotation, scale), color);
            }
            else if (killerCol is SphereCollider)
            {
                SphereCollider sc = killerCol as SphereCollider;
                float scalar = Mathf.Max(Mathf.Abs(Mathf.Max(sc.transform.localScale.x, sc.transform.localScale.y)), Mathf.Abs(sc.transform.localScale.z));
                OhMyGizmos.Sphere(sc.bounds.center, sc.radius * scalar, color);
            }
        }
    }
}