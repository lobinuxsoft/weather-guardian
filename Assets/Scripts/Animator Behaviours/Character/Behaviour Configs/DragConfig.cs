using UnityEngine;

namespace WeatherGuardian.Behaviours.Configs
{
    [CreateAssetMenu(fileName = nameof(DragConfig), menuName = "TesisBand/Behaviours/Configs/Drag Config")]
    public class DragConfig : ScriptableObject
    {
        [field: Header("Drag:")]
        [field: SerializeField] public AnimationCurve Drag { get; private set; } = new AnimationCurve(new Keyframe[] { new Keyframe(0.0f, 10.0f), new Keyframe(1.0f, 5.0f) });
    }
}