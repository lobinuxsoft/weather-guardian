using System;
using UnityEngine;
using UnityEngine.UIElements;
using WeatherGuardian.Utils;

namespace WeatherGuardian.Obstacles 
{
    [RequireComponent(typeof(SplineFollowPath))]
    public class HorizontalObstacle : Obstacle
    {
        [SerializeField] [Range(0.05f, 10.0f)] private float timeStoped = 1.0f;

        public event Action OnStartMoving; 

        private SplineFollowPath path;

        private Timer timerForTimeStoped;

        public SplineFollowPath Path 
        {
            get 
            {
                return path;
            }
        }

        private void Awake()
        {
            path = GetComponent<SplineFollowPath>();

            timerForTimeStoped = new Timer(timeStoped, false);
        }

        private void Start()
        {
            path.Rotate = false;            

            path.OnHalfPath += StopMoving;

            timerForTimeStoped.OnTimerEnds += StartMoving;           
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
            timerForTimeStoped.UpdateTimerWithResetAndStop();            
        }

        private void StartMoving() 
        {
            path.Moving = true;

            OnStartMoving?.Invoke();
        }

        private void StopMoving() 
        {
            path.Moving = false;

            timerForTimeStoped.IsRunning = true;
        }
    }
}