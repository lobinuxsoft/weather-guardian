using CryingOnion.Timeline.Conditions;
using UnityEngine.Playables;

namespace CryingOnion.Timeline.Behaviour
{
    [System.Serializable]
    public class ConditionalLoopBehaviour : PlayableBehaviour
    {
        public ScriptableCondition condition;

        public float start;
        public float end;
    }
}