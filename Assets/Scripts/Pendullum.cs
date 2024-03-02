using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class Pendullum : MonoBehaviour
    {
        enum Axis { Forward, Right, Up }

        [SerializeField] private AnimationCurve pendullumBehaviour;
        [SerializeField] private float angleA;
        [SerializeField] private float angleB;
        [SerializeField] private Axis axisToRotate = Axis.Forward;
        [SerializeField] private float speed = 1;

        private Rigidbody body;

        private float pendullumTime;

        Vector3 axis;
        Quaternion rotA;
        Quaternion rotB;

        public float PendullumTime 
        {
            get 
            {
                return pendullumTime;
            }
        }

        private void Awake()
        {
            body = GetComponent<Rigidbody>();
            body.isKinematic = true;

            switch (axisToRotate)
            {
                case Axis.Forward:
                    axis = transform.forward;
                    break;
                case Axis.Right:
                    axis = transform.right;
                    break;
                case Axis.Up:
                    axis = transform.up;
                    break;
            }
        }

        // Update is called once per frame
        void Update()
        {
            rotA = Quaternion.AngleAxis(angleA, axis);
            rotB = Quaternion.AngleAxis(angleB, axis);

            pendullumTime = pendullumBehaviour.Evaluate(Mathf.PingPong(Time.time * speed, 1));

            body.MoveRotation(Quaternion.LerpUnclamped(rotA, rotB, pendullumTime));            
        }
    }
}