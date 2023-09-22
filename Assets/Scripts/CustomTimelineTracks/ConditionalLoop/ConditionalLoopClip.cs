using CryingOnion.Timeline.Behaviour;
using CryingOnion.Timeline.Conditions;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryingOnion.Timeline.Clip
{
    [System.Serializable]
    public class ConditionalLoopClip : PlayableAsset, ITimelineClipAsset
    {
        [HideInInspector]
        public ConditionalLoopBehaviour template = new ConditionalLoopBehaviour();

        public ScriptableCondition condition;
        public float start;
        public float end;

        public ClipCaps clipCaps => ClipCaps.None;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<ConditionalLoopBehaviour>.Create(graph, template);
            ConditionalLoopBehaviour clone = playable.GetBehaviour();
            clone.condition = condition;
            clone.start = start;
            clone.end = end;
            return playable;
        }
    }
}