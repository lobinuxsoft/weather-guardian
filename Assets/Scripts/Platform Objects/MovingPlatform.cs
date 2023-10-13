using UnityEngine;
using UnityEngine.SocialPlatforms;
using WeatherGuardian.Interfaces;

namespace WeatherGuardian.PlatformObjects
{
    [RequireComponent(typeof(SplineFollowPath))]
    public class MovingPlatform : MonoBehaviour, IMoveStrategy
    {
        [SerializeField][Range(10.0f, 200.0f)] private float maxSeparationDistance = 20.0f;

        private SplineFollowPath path;

        private Transform playerTransform;

        private void Awake()
        {
            path = GetComponent<SplineFollowPath>();

            path.OnHalfPath += StopMoving;
        }

        private void Start()
        {
            path.Rotate = false;

            path.Moving = false;

            playerTransform = null;
        }

        private void OnDestroy()
        {
            path.OnHalfPath -= StopMoving;
        }

        private void Update()
        {
            MoveBehaviour();
        }
        public void MoveBehaviour()
        {
            if (playerTransform != null)
            {
                if (Vector3.Distance(playerTransform.transform.position, transform.position) > maxSeparationDistance)
                {
                    playerTransform = null;

                    path.ResetPath();
                }
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag == "Player")
            {
                if (playerTransform != collision.transform)
                {
                    playerTransform = collision.transform;
                }

                path.Moving = true;
            }
        }

        private void StopMoving()
        {
            path.Moving = false;
        }
    }
}