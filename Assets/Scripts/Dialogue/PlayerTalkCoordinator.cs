using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerTalkCoordinator : MonoBehaviour
{
    [SerializeField]
    LayerMask npcLayerMask = 9;

    [SerializeField]
    LayerMask teleporterLayerMask = 9;

    [SerializeField]
    TalkableNPC currTalkableNPC_ = null;

    UIManager uiManager_ = null;
    FlagManager flagManager_;

    TeleportPoint lastTpTarget_ = null;

    public enum PlayerDialogueState
    {
        Inactive = 0,
        InDialogue = 1,
    }

    private PlayerDialogueState state_ = PlayerDialogueState.Inactive;
    public PlayerDialogueState State { get { return state_; } }

    Func<bool> dialogueCompletedCb_;

    // Start is called before the first frame update
    void Start()
    {
        uiManager_ = UIManager.Instance;
        flagManager_ = FlagManager.Instance;

        // If NPC still has dialogue, show it as interactible.
        UnityAction<FlagManager.EventFlag> checkStillInteractible =
            new UnityAction<FlagManager.EventFlag>((FlagManager.EventFlag flag) =>
            {
                if (currTalkableNPC_ == null)
                    return;

                currTalkableNPC_.SetInteractable(currTalkableNPC_.HasAvailableModel());
            });
        flagManager_.AddListener(checkStillInteractible);
        uiManager_.Player = this;

        dialogueCompletedCb_ = () =>
        {
            state_ = PlayerDialogueState.Inactive;
            return true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Talk"))
        {
            if (currTalkableNPC_ == null)
                return;

            SetTalking(currTalkableNPC_.Model);
        }
    }

    public bool TeleportTo(Vector3 position, TeleportPoint activatingTpPoint, TeleportPoint targetTPpoint)
    {
        if (activatingTpPoint == lastTpTarget_)
            return false;

        lastTpTarget_ = targetTPpoint;
        transform.position = position;
        return true;
    }

    public void SetTalking(DialogueModel model)
    {
        if (state_ == PlayerDialogueState.InDialogue)
            return;

        if (uiManager_.SetAndShowDialogue(model, dialogueCompletedCb_))
        {
            state_ = PlayerDialogueState.InDialogue;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (npcLayerMask == (npcLayerMask | (1 << other.gameObject.layer)))
        {
            TalkableNPC npc = other.GetComponent<TalkableNPC>();
            if (npc == null)
            {
                Debug.LogError("Entered trigger of chracter who is not NPC.");
                return;
            }

            if (currTalkableNPC_ != null)
            {
                Debug.LogWarning("Overriding current NPC to talk to.");
                currTalkableNPC_.SetInteractable(false);
            }

            currTalkableNPC_ = npc;
            if (!currTalkableNPC_.HasAvailableModel())
            {
                Debug.LogWarning("This NPC somehow has no avialable dialogue.");
                return;
            }
            currTalkableNPC_.SetInteractable(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (npcLayerMask == (npcLayerMask | (1 << other.gameObject.layer)))
        {
            TalkableNPC npc = other.GetComponent<TalkableNPC>();
            if (npc == null)
            {
                Debug.LogError("Exited trigger of chracter who is not NPC.");
                return;
            }

            if (currTalkableNPC_ != npc)
            {
                Debug.LogWarning("Exiting NPC range of an NPC you weren't in range of.");
                npc.SetInteractable(false);
            }

            currTalkableNPC_.SetInteractable(false);
            currTalkableNPC_ = null;
        }
        else if (teleporterLayerMask == (teleporterLayerMask | (1 << other.gameObject.layer)))
        {
            if (other.GetComponent<TeleportPoint>() == lastTpTarget_)
                lastTpTarget_ = null;
        }
    }
}
