using UnityEngine;

using WeatherGuardian.Interfaces;

namespace WeatherGuardian.Obstacles
{
    public abstract class Obstacle : MonoBehaviour, IMoveStrategy
    {
        public abstract void MoveBehaviour();
    }
}