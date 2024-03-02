using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        Camera cam;

        private void Awake() => cam = Camera.main;

        private void Update() => transform.rotation = Quaternion.LookRotation(transform.position - cam.transform.position);
    }
}