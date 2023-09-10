using UnityEngine;
namespace CryingOnion.Timeline.Conditions
{
    [CreateAssetMenu(fileName = "New Bool Condition", menuName = "CryingOnion/Timeline/Conditions/Bool Condition")]
    public class BoolCondition : ScriptableCondition
    {
        [field: SerializeField] public bool Value { get; set; } = false;

        public override bool ConditionMet() => Value;
    }
}