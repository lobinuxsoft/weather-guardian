using System;
using UnityEngine;

namespace WeatherGuardian.Utils
{
    public class Timer
    {
        public event Action OnTimerEnds;

        private float timerDuration;

        private float time;

        public Timer(float timerDuration)
        {
            TimerDuration = timerDuration;
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
            if (time > 0.0f)
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
            if (time > 0.0f)
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