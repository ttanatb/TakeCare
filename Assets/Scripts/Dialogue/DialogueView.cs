using System.Collections;
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

    private HashSet<char> charsToIgnoreForTicks_ = new HashSet<char>();
    private HashSet<char> charsToLowerVoice_ = new HashSet<char>();
    private HashSet<char> charsToStopLoweringVoice_ = new HashSet<char>();

    [SerializeField]
    float lowerVoiceVolumeFactor_ = 0.3f;

    [SerializeField]
    float shoutVoiceVolumeFactor_ = 1.15f;

    bool isCurrentlyLoweringVoice = false;

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
            if (value != null)
            {
                scrollIntervalSec_ = 1.0f / value.Speed;
                nameText_.text = value.Name;
            }
        }
    }

    public List<Func<bool>> DialogueCompletedCbs
    {
        get { return dialogueCompletedCbs_; }
        set { dialogueCompletedCbs_ = value; }
    }

    FlagManager flagManager_;
    VolumeManager volumeManager_;

    [SerializeField]
    bool isWeb = true;

    private void Awake()
    {
        if (renderers_ == null || renderers_.Length == 0)
            renderers_ = GetComponentsInChildren<Image>();

        char[] charsToIgnore = { ',', '.', ' ', '\n', '!', '-', '(', '{', '[', ')', '}', ']', '*', };
        foreach (char c in charsToIgnore)
            charsToIgnoreForTicks_.Add(c);

        char[] lowerVoiceChar = { '(', '{', '[', };
        foreach (char c in lowerVoiceChar)
            charsToLowerVoice_.Add(c);

        char[] stopLowerVoiceChar = { ')', '}', ']', };
        foreach (char c in stopLowerVoiceChar)
            charsToStopLoweringVoice_.Add(c);
    }

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        volumeManager_ = VolumeManager.Instance;
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

        isWeb = Application.platform != RuntimePlatform.WebGLPlayer;

    }

    // Update is called once per frame
    void Update()
    {
        if (isWeb ? IsTalkInputUpWeb() : IsTalkInputUpPC())
        {
            ignoreNextInput_ = false;
        }
        switch (state_)
        {
            case State.Scrolling:
                ScrollText();
                break;
            default:
                break;
        }
    }

    private bool IsTalkInputUpWeb()
    {
        return Input.GetMouseButtonUp(0);
    }

    private bool IsTalkInputUpPC()
    {
        return Input.GetButtonUp("Talk");
    }

    // Starts the text scrolling.
    public bool StartDialogueScroll()
    {
        // Proceed until we find a dialogue we can hit.
        while (true)
        {
            if (dialogueIndex_ >= model_.Dialogue.Length)
                return false;

            if (!flagManager_.GetFlagCompletion(model_.Dialogue[dialogueIndex_].PrereqFlag) ||
                !flagManager_.GetFlagUnmet(model_.Dialogue[dialogueIndex_].PrereqUnmetFlags))
                GoToNext(false);
            else break;
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
            isCurrentlyLoweringVoice = false;
            //Debug.Log("Skipped to end, ignoring next input.");
        }
        else if (state_ == State.Waiting)
        {
            GoToNext();
        }
    }

    public void GoToNext(bool assignIgnoreNext = true)
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
            if (assignIgnoreNext)
                ignoreNextInput_ = true;
            //Debug.Log("End of dialogue box, ignoring next input.");
        }
        else if (state_ != State.Initial)
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
        isCurrentlyLoweringVoice = false;
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
            char currChar = text[subStringLength_ - 1];

            // Audio volume control based on brackets
            if (charsToLowerVoice_.Contains(currChar))
                isCurrentlyLoweringVoice = true;
            else if (charsToStopLoweringVoice_.Contains(currChar))
                isCurrentlyLoweringVoice = false;

            bool isShouting = false;
            if (subStringLength_ > 1)
            {
                isShouting =
                    char.IsUpper(currChar) && char.IsUpper(text[subStringLength_ - 2]);
            }

            float volume = volumeManager_.SoundEffectVolume * model_.Volume
                + UnityEngine.Random.Range(-model_.VolumeVariance, model_.VolumeVariance);
            if (isCurrentlyLoweringVoice)
            {
                volume *= lowerVoiceVolumeFactor_;
            }
            if (isShouting)
            {
                volume *= shoutVoiceVolumeFactor_;
            }

            tickAudioSource_.volume = volume;
            tickAudioSource_.pitch = model_.Pitch
                + UnityEngine.Random.Range(-model_.PitchVariance, model_.PitchVariance);

            // Play audio
            if (!charsToIgnoreForTicks_.Contains(currChar))
            {
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
