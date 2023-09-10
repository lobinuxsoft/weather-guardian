using CryingOnion.Timeline.Conditions;
using UnityEngine.Playables;

namespace CryingOnion.Timeline.Behaviour
{
    [System.Serializable]
    public class ConditionalLoopBehaviour : PlayableBehaviour
    {
        public ScriptableCondition condition;

        public double start;
        public double end;
    }
}