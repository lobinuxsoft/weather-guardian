using CryingOnion.GizmosRT.Runtime;
using UnityEngine;

namespace WeatherGuardian.Utils
{
    [RequireComponent(typeof(BoxCollider))]
    public class KillerTrigger : MonoBehaviour
    {
        [SerializeField] private DetectionType detectionType;

        [Header("Only for debug mode")]
        [SerializeField] private Color debugColor = Color.red;
        
        private BoxCollider boxTrigger;

        private void Awake()
        {
            boxTrigger = GetComponent<BoxCollider>();
            boxTrigger.isTrigger = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if ((detectionType & DetectionType.ENTER) == 0) return;

            CheckPoint.JumpToLastCheckPoint(other.gameObject);
        }

        private void OnTriggerStay(Collider other)
        {
            if ((detectionType & DetectionType.STAY) == 0) return;

            CheckPoint.JumpToLastCheckPoint(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if ((detectionType & DetectionType.EXIT) == 0) return;

            CheckPoint.JumpToLastCheckPoint(other.gameObject);
        }

        private void LateUpdate() => GizmosRT.Bounds(boxTrigger.bounds, debugColor);
    }
}