using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public class DialogueOption
{
    public FlagManager.EventFlag[] RequiredFlagsForDialogue;
    public string DialogueText;
    public FlagManager.EventFlag[] FlagsToMarkAsComplete;
}

[System.Serializable]
public class Dialogue
{
    public FlagManager.EventFlag[] PrereqFlag;
    public FlagManager.EventFlag[] PrereqUnmetFlags;
    public string Text;
    public DialogueOption[] Options;
    public FlagManager.EventFlag[] FlagsToMarkAsComplete;
}

[System.Serializable]
public class DialogueModel
{
    public FlagManager.EventFlag[] PrereqFlags;
    public FlagManager.EventFlag[] PrereqUnmetFlags;
    public string Name;
    public Dialogue[] Dialogue;
    public float Speed = 15.0f;
    public float Pitch = 1.0f;
    public float PitchVariance = 0.1f;
    public float Volume = 1.0f;
    public float VolumeVariance = 0.1f;
    public AudioClip TickAudioClip;
    public FlagManager.EventFlag[] FlagsToMarkComplete;
}

public class TalkableNPC : MonoBehaviour
{
    FlagManager flagManager_;

    [SerializeField]
    DialogueModel[] models_;

    public DialogueModel Model
    {
        get
        {
            for (int i = models_.Length - 1; i > -1; i--)
            {
                if (flagManager_.GetFlagCompletion(models_[i].PrereqFlags) &&
                    flagManager_.GetFlagUnmet(models_[i].PrereqUnmetFlags))
                    return models_[i];
            }
            return null;
        }
    }

    public bool HasAvailableModel()
    {
        for (int i = models_.Length - 1; i > -1; i--)
        {
            if (flagManager_.GetFlagCompletion(models_[i].PrereqFlags) &&
                flagManager_.GetFlagUnmet(models_[i].PrereqUnmetFlags))
                return true;
        }
        return false;
    }

    [SerializeField]
    TextMeshPro interactbleText_ = null;

    private void Start()
    {
        flagManager_ = FlagManager.Instance;

        if (interactbleText_ == null)
            interactbleText_ = GetComponentInChildren<TextMeshPro>();
    }

    public void SetInteractable(bool isInteractable)
    {
        interactbleText_.gameObject.SetActive(isInteractable);
    }
}
