
using UnityEngine;

namespace CryingOnion.Timeline.Conditions
{
    public abstract class ScriptableCondition:ScriptableObject
    {
        public abstract bool ConditionMet();
    }
}