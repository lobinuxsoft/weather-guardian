using System;
using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class Timer
    {
        public event Action OnTimerEnds;

        private float timerDuration;

        private float time;

        private bool isRunning;

        public float CurrentTime 
        {
            get 
            {
                return time;
            }
        }

        public bool IsRunning 
        {
            set 
            { 
                isRunning = value;
            }
            get 
            {
                return isRunning;
            }
        }

        public Timer(float timerDuration, bool isRunning)
        {
            TimerDuration = timerDuration;

            this.isRunning = isRunning;
        }

        public Timer(float timerDuration)
        {
            TimerDuration = timerDuration;

            isRunning = true;
        }

        public float TimerDuration
        {
            set
            {
                timerDuration = value;

                time = timerDuration;
            }
            get
            {
                return timerDuration;
            }
        }

        public void UpdateTimer()
        {
            if (time > 0.0f && isRunning)
            {
                time -= Time.deltaTime;

                if (time <= 0.0f)
                {
                    OnTimerEnds?.Invoke();
                }
            }
        }

        public void UpdateTimerWithReset() 
        {
            if (time > 0.0f && isRunning)
            {
                time -= Time.deltaTime;

                if (time <= 0.0f)
                {
                    OnTimerEnds?.Invoke();

                    ResetTimer();
                }
            }
        }

        public void ResetTimer()
        {
            time = timerDuration;
        }
    }
}