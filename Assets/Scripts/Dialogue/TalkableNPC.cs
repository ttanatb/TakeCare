using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEditor;
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
    string soAssetName;

    [SerializeField]
    DialogueModelSO soModel_;

    public DialogueModel Model
    {
        get
        {
            for (int i = soModel_.DialogueModels.Length - 1; i > -1; i--)
            {
                if (flagManager_.GetFlagCompletion(soModel_.DialogueModels[i].PrereqFlags) &&
                    flagManager_.GetFlagUnmet(soModel_.DialogueModels[i].PrereqUnmetFlags))
                    return soModel_.DialogueModels[i];
            }
            return null;
        }
    }

    public bool HasAvailableModel()
    {
        for (int i = soModel_.DialogueModels.Length - 1; i > -1; i--)
        {
            if (flagManager_.GetFlagCompletion(soModel_.DialogueModels[i].PrereqFlags) &&
                flagManager_.GetFlagUnmet(soModel_.DialogueModels[i].PrereqUnmetFlags))
                return true;
        }
        return false;
    }

    [SerializeField]
    TextMeshPro interactbleText_ = null;

    private void Start()
    {
        flagManager_ = FlagManager.Instance;

#if UNITY_EDITOR
        if (soModel_ == null && soAssetName != "")
        {
            soModel_ = (DialogueModelSO)AssetDatabase.
                LoadAssetAtPath("Assets/DialogueModels/" +
                soAssetName + ".asset", typeof(DialogueModelSO));

        }
#endif
        if (interactbleText_ == null)
        {
            interactbleText_ = GetComponentInChildren<TextMeshPro>();
        }
        SetInteractable(false);
    }

    public void SetInteractable(bool isInteractable)
    {
        interactbleText_.gameObject.SetActive(isInteractable);
    }
}
