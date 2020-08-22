﻿using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class DialogueView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText_ = null;

    [SerializeField]
    TextMeshProUGUI dialogueText_ = null;

    [SerializeField]
    AudioSource tickAudioSource_ = null;

    [SerializeField]
    DialogueOptionView dialogueOptionView_ = null;

    private HashSet<char> charsToIgnoreForTicks = new HashSet<char>();

    bool ignoreNextInput_ = false;

    [SerializeField]
    private Image[] renderers_;

    public enum State
    {
        Initial = 0,
        Scrolling = 1,
        Waiting = 2,
        WaitingForOptionSelection = 3,
        End = 4,
    }

    private State state_;
    private int dialogueIndex_ = 0;
    private int subStringLength_ = 0;
    private float timer_ = 0.0f;
    private float scrollIntervalSec_ = 0.0f;

    public State CurrentState { get { return state_; } }

    // Model
    [SerializeField]
    DialogueModel model_;

    private List<Func<bool>> dialogueCompletedCbs_;

    public DialogueModel Model
    {
        get { return model_; }
        set
        {
            model_ = value;
            state_ = State.Initial;
            dialogueIndex_ = 0;
            ClearText();
            scrollIntervalSec_ = 1.0f / value.Speed;
            nameText_.text = value.Name;
        }
    }

    public List<Func<bool>> DialogueCompletedCbs
    {
        get { return dialogueCompletedCbs_; }
        set { dialogueCompletedCbs_ = value; }
    }

    FlagManager flagManager_;

    private void Awake()
    {
        if (renderers_ == null || renderers_.Length == 0)
            renderers_ = GetComponentsInChildren<Image>();

        char[] charsToIgnore = { ',', '.', ' ', '\n', '!', '-' };
        foreach (char c in charsToIgnore)
            charsToIgnoreForTicks.Add(c);
    }

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        dialogueOptionView_.OptionSelectedCb = () =>
        {
            if (state_ != State.WaitingForOptionSelection)
            {
                Debug.LogWarning("OptionSelectedCb called when DialogueView isn't waiting for input?");
                return false;
            }

            GoToNext();
            return true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        if (ignoreNextInput_)
            ignoreNextInput_ = false;

        switch (state_)
        {
            case State.Scrolling:
                ScrollText();
                break;
            default:
                break;
        }
    }


    // Starts the text scrolling.
    public bool StartDialogueScroll()
    {
        // Check if flags are met for dialogue.
        if (!flagManager_.GetFlagCompletion(model_.Dialogue[dialogueIndex_].PrereqFlag) ||
            !flagManager_.GetFlagUnmet(model_.Dialogue[dialogueIndex_].PrereqUnmetFlags))
        {
            GoToNext();
        }

        //Debug.Log("Attempgint to start dialogue scroll");
        if (ignoreNextInput_)
        {
            //Debug.Log("Can't scroll dialogue, just completed something.");
            return false;
        }

        state_ = State.Scrolling;
        ignoreNextInput_ = true;
        //Debug.Log("Scrolling dialogue now.");
        return true;
    }

    // Shows next dialogue.
    public void SkipToEndOrGoToNext()
    {
        //Debug.Log("SkipToEndOrGoToNext");
        if (ignoreNextInput_)
        {
            //Debug.Log("SkipToEndOrGoToNext ignored");
            return;
        }
        // Skip to end of scrolling
        if (state_ == State.Scrolling)
        {
            dialogueText_.text = model_.Dialogue[dialogueIndex_].Text;
            subStringLength_ = model_.Dialogue[dialogueIndex_].Text.Length;
            timer_ = 0.0f;
            ShowOptionsIfExist();
            ignoreNextInput_ = true;
            //Debug.Log("Skipped to end, ignoring next input.");
        }
        else if (state_ == State.Waiting)
        {
            GoToNext();
        }
    }

    public void GoToNext()
    {
        // Clear view for next dialogue.
        dialogueIndex_++;
        ClearText();

        // If at the dialogue.
        //Debug.Log("Curr dialogue index: " + dialogueIndex_);
        if (dialogueIndex_ >= model_.Dialogue.Length)
        {
            //Debug.Log("End state!");
            state_ = State.End;
            if (dialogueCompletedCbs_ != null)
            {
                foreach (Func<bool> cb in dialogueCompletedCbs_)
                    cb.Invoke();
            }
            ignoreNextInput_ = true;
            //Debug.Log("End of dialogue box, ignoring next input.");
        }
        else
        {
            StartDialogueScroll();
            //Debug.Log("Showing next dialogue, ignoring next input.");
        }
    }

    public void SetRenderersEnabled(bool isEnabled)
    {
        foreach (Image r in renderers_)
            r.enabled = isEnabled;

        nameText_.enabled = isEnabled;
        dialogueText_.enabled = isEnabled;
    }

    private void ClearText()
    {
        dialogueText_.text = "";
        timer_ = 0.0f;
        subStringLength_ = 0;
    }

    // Scrolls through text
    private void ScrollText()
    {
        timer_ += Time.deltaTime;
        if (timer_ > scrollIntervalSec_)
        {
            string text = model_.Dialogue[dialogueIndex_].Text;

            subStringLength_++;
            // Reached the end of text.
            if (subStringLength_ > text.Length)
            {
                ShowOptionsIfExist();
                return;
            }

            dialogueText_.text = text.Substring(0, subStringLength_);
            if (!charsToIgnoreForTicks.Contains(text[subStringLength_ - 1]))
            {
                tickAudioSource_.volume = model_.Volume
                    + UnityEngine.Random.Range(-model_.VolumeVariance, model_.VolumeVariance);
                tickAudioSource_.pitch = model_.Pitch
                    + UnityEngine.Random.Range(-model_.PitchVariance, model_.PitchVariance);
                tickAudioSource_.PlayOneShot(model_.TickAudioClip);
            }
            timer_ = 0.0f;
        }
    }

    private void ShowOptionsIfExist()
    {
        state_ = State.Waiting;
        if (model_.Dialogue[dialogueIndex_].Options == null ||
            model_.Dialogue[dialogueIndex_].Options.Length == 0)
            return;

        dialogueOptionView_.DialogueOptions = model_.Dialogue[dialogueIndex_].Options;
        state_ = State.WaitingForOptionSelection;
    }
}
