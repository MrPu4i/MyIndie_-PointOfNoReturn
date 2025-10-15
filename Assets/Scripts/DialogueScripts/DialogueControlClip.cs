using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]

public class DialogueControlClip : PlayableAsset
{
    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        var playable = ScriptPlayable<DialogueControlBehaviour>.Create(graph);
        return playable;
    }

    [System.Serializable]
    public class DialogueControlBehaviour : PlayableBehaviour
    {
        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            DialogueManager.Instance.ShowCurrentLine();
        }
    }
}
