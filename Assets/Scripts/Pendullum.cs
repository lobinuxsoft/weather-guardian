using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class Pendullum : MonoBehaviour
    {
        [SerializeField] private AnimationCurve pendullumBehaviour;
        [SerializeField] private Quaternion rotationA;
        [SerializeField] private Quaternion rotationB;
        [SerializeField] private float speed = 1;

        private Rigidbody body;

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {
            body.MoveRotation(Quaternion.LerpUnclamped(rotationA, rotationB, pendullumBehaviour.Evaluate(Mathf.PingPong(Time.time * speed, 1))));
        }
    }
}