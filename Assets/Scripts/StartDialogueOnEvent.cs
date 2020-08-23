using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDialogueOnEvent : MonoBehaviour
{
    [SerializeField]
    PlayerTalkCoordinator player_;

    [SerializeField]
    int modelIndex_;

    [SerializeField]
    FlagManager.EventFlag flagToListenFor_;

    FlagManager flagManager_ = null;
    TalkableNPC talkableNPC = null;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        flagManager_.AddListener(OnEventFlagCompleted);
        talkableNPC = GetComponent<TalkableNPC>();
    }

    // Update is called once per frame
    void OnEventFlagCompleted(FlagManager.EventFlag flag)
    {
        if (flagToListenFor_ == flag)
        {
            Debug.Log("Attempting to start player dialogue");
            player_.SetTalking(talkableNPC.Model);
        }
    }
}
