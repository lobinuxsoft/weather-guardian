using UnityEngine;
using UnityEngine.SocialPlatforms;
using WeatherGuardian.Interfaces;

namespace WeatherGuardian.PlatformObjects
{
    [RequireComponent(typeof(SplineFollowPath))]
    public class MovingPlatform : MonoBehaviour, IMoveStrategy
    {
        [SerializeField] private FMODUnity.EventReference startMovementSfx;

        [SerializeField] private FMODUnity.EventReference loopMovementSfx;

        [SerializeField] private FMODUnity.EventReference endMovementSfx;

        [SerializeField][Range(0f, 200.0f)] private float maxSeparationDistance = 20.0f;       

        private SplineFollowPath path;

        private Transform playerTransform;

        private void Awake()
        {
            path = GetComponent<SplineFollowPath>();

            path.OnHalfPath += StopMoving;

            path.OnEnd += path.ResetPath;

            path.OnEnd += CallStopMovingPlatformSfx;
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

            path.OnEnd -= path.ResetPath;

            path.OnEnd -= CallStopMovingPlatformSfx;
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
            if (collision.transform.tag == "Player" && !path.Moving)
            {
                playerTransform = collision.transform;

                //Try to prevent cat bouncing with platform
                Rigidbody platformRigidBody = collision.gameObject.GetComponent<Rigidbody>();

                Rigidbody catRigidBody = gameObject.GetComponent<Rigidbody>();

                platformRigidBody.velocity = Vector3.zero;

                catRigidBody.velocity = Vector3.zero;

                CallStartMovingPlatformSfx();

                path.Moving = true;
            }
        }

        private void StopMoving()
        {
            CallStopMovingPlatformSfx();

            path.Moving = false;
        }

        private void CallStartMovingPlatformSfx() 
        {           
            if (!startMovementSfx.IsNull && !path.Moving)
                FMODUnity.RuntimeManager.PlayOneShotAttached(startMovementSfx, gameObject);

            if (!startMovementSfx.IsNull && !path.Moving)
                FMODUnity.RuntimeManager.PlayOneShotAttached(loopMovementSfx, gameObject);
        }

        private void CallStopMovingPlatformSfx() 
        {
            if (!endMovementSfx.IsNull)
                FMODUnity.RuntimeManager.PlayOneShotAttached(endMovementSfx, gameObject);
        }
    }
}