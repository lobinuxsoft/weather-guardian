using System;

using UnityEngine;

namespace WeatherGuardian.Pause
{
    public class PauseInputDetection : MonoBehaviour
    {
        [SerializeField] private KeyCode pauseKey;

        public event Action onPauseTriggered;
        
        void Update()
        {
            if (Input.GetKeyDown(pauseKey)) 
            {
                onPauseTriggered?.Invoke();
            }        
        }
    }
}