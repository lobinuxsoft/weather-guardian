using UnityEngine;
using UnityEngine.Playables;

namespace CryingOnion.Timeline.Behaviour
{
    public class ConditionalLoopMixerBehaviour : PlayableBehaviour
    {
        private PlayableDirector director;

        public override void OnPlayableCreate(Playable playable)
        {
            director = (playable.GetGraph().GetResolver() as PlayableDirector);
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            int inputCount = playable.GetInputCount();

            for (int i = 0; i < inputCount; i++)
            {
                ScriptPlayable<ConditionalLoopBehaviour> inputPlayable = (ScriptPlayable<ConditionalLoopBehaviour>)playable.GetInput(i);
                ConditionalLoopBehaviour input = inputPlayable.GetBehaviour();

                if (input.condition && input.condition.ConditionMet() && Mathf.Abs((float)(input.end - director.time)) < 0.01f)
                {
                    director.time = input.start;
                }
            }
        }
    }
}