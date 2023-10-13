using UnityEngine;

namespace WeatherGuardian.Obstacles 
{
    [RequireComponent(typeof(SplineFollowPath))]
    public class HorizontalObstacle : Obstacle
    {
        [SerializeField] [Range(1.0f, 10.0f)] private float timeStoped = 1.0f;

        private SplineFollowPath path;
        
        float timer = 0.0f;

        private void Start()
        {
            path = GetComponent<SplineFollowPath>();

            path.Rotate = false;

            path.Moving = true;

            path.OnHalfPath += StopMoving;
        }

        private void OnDestroy()
        {
            path.OnHalfPath -= StopMoving;
        }

        private void Update()
        {
            MoveBehaviour();
        }

        public override void MoveBehaviour()
        {
            if (timer > 0.0f) 
            {
                timer -= Time.deltaTime;

                if (timer <= 0.0f) 
                {
                    timer = 0.0f;

                    StartMoving();
                }
            }
        }

        private void StartMoving() 
        {
            path.Moving = true;
        }

        private void StopMoving() 
        {
            path.Moving = false;

            timer = timeStoped;
        }
    }
}