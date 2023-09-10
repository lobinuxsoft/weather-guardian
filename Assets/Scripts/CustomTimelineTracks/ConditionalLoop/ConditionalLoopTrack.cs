using CryingOnion.Timeline.Clip;
using CryingOnion.Timeline.Behaviour;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace CryingOnion.Timeline.Track
{
    [TrackColor(0.7366781f, 0.3261246f, 0.8529412f)]
    [TrackClipType(typeof(ConditionalLoopClip))]
    public class ConditionalLoopTrack : TrackAsset
    {
        public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
        {
            ScriptPlayable<ConditionalLoopMixerBehaviour> scriptPlayable = ScriptPlayable<ConditionalLoopMixerBehaviour>.Create(graph, inputCount);

            ConditionalLoopMixerBehaviour b = scriptPlayable.GetBehaviour();

            //This foreach will rename clips based on what they do, and collect the markers and put them into a dictionary
            //Since this happens when you enter Preview or Play mode, the object holding the Timeline must be enabled or you won't see any change in names
            foreach (var c in GetClips())
            {
                ConditionalLoopClip clip = (ConditionalLoopClip)c.asset;
                clip.end = c.end;
                clip.start = c.start;
                string displayName = clip.condition != null ? clip.condition.name : "NULL";
                c.displayName = $"↩ {displayName}";
            }

            return scriptPlayable;
        }
    }
}