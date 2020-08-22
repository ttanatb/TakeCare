using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;

public class DialogueView : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText_;

    [SerializeField]
    TextMeshProUGUI dialogueText_;

    [SerializeField]
    AudioSource tickAudioSource_;

    [SerializeField]
    float baseVolume_ = 1.0f;
    [SerializeField]
    float volumeVariance_ = 0.1f;
    [SerializeField]
    float basePitch_ = 1.0f;
    [SerializeField]
    float pitchVariance_ = 0.1f;

    [SerializeField]
    AudioClip tickAudioClip_;

    private const float DEFAULT_SCROLL_SPEED = 12.5f;
    private HashSet<char> charsToIgnoreForTicks = new HashSet<char>();

    enum State
    {
        Invalid = 0,
        Scrolling = 1,
        Waiting = 2,
        End = 3,
    }

    private State state_;
    private int dialogueIndex_ = 0;
    private int subStringLength_ = 0;
    private float timer_ = 0.0f;
    private float scrollIntervalSec_ = 1.0f / DEFAULT_SCROLL_SPEED;

    // Model
    private string[] dialogues_;
    private UnityEvent completedEvent_;

    public string Name
    {
        get { return nameText_.text; }
        set { nameText_.text = value; }
    }

    public string[] Dialogue
    {
        get { return dialogues_; }
        set { dialogues_ = value; }
    }

    public UnityEvent CompletedEvent
    {
        get { return completedEvent_; }
        set { completedEvent_ = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        char[] charsToIgnore = { ',', '.', ' ', '\n', '!', };
        foreach (char c in charsToIgnore)
            charsToIgnoreForTicks.Add(c);


        // TESTING
        Name = "TestNAME";
        string[] testDialogue = {
            "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse tincidunt, diam eget bibendum aliquet, erat ligula placerat sapien, nec gravida odio lacus non magna. Aenean at libero sit amet lectus posuere pretium vitae sit amet massa. Vivamus id odio neque. Phasellus non luctus diam, non rutrum lectus. Suspendisse eget neque sed felis rhoncus efficitur. Praesent sem lorem, fermentum sit amet purus a, mollis dignissim risus.",
            "Praesent id tortor vitae nisi iaculis condimentum ege",
            "ltricies......... Donec faucibus metu",
        };
        Dialogue = testDialogue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            StartDialogueScroll();

        if (Input.GetKeyDown(KeyCode.R))
            Next();

        if (Input.GetKeyDown(KeyCode.T))
            SkipToEnd();

        switch (state_)
        {
            case State.Invalid:
                break;
            case State.Scrolling:
                ScrollText();
                break;
            case State.Waiting:
                break;
        }
    }


    // Starts the text scrolling.
    public void StartDialogueScroll()
    {
        state_ = State.Scrolling;
    }

    // Shows next dialogue.
    public void Next()
    {
        // Clear view for next dialogue.
        dialogueIndex_++;
        dialogueText_.text = "";
        state_ = State.Scrolling;
        timer_ = 0.0f;
        subStringLength_ = 0;

        // If at the dialogue.
        if (dialogueIndex_ >= dialogues_.Length)
        {
            state_ = State.End;
            if (completedEvent_ != null)
                completedEvent_.Invoke();
        }
        else
            state_ = State.Scrolling;
    }

    // Skips to end of dialogue.
    public void SkipToEnd()
    {
        dialogueText_.text = dialogues_[dialogueIndex_];
        subStringLength_ = dialogues_[dialogueIndex_].Length;
        timer_ = 0.0f;
        state_ = State.Waiting;
    }


    // Scrolls through text
    private void ScrollText()
    {
        timer_ += Time.deltaTime;
        if (timer_ > scrollIntervalSec_)
        {
            string text = dialogues_[dialogueIndex_];

            subStringLength_++;
            // Reached the end of text.
            if (subStringLength_ >= text.Length)
            {
                state_ = State.Waiting;
                return;
            }

            if (!charsToIgnoreForTicks.Contains(text[subStringLength_]))
            {
                tickAudioSource_.volume = baseVolume_ + Random.Range(-volumeVariance_, volumeVariance_);
                tickAudioSource_.pitch = basePitch_ + Random.Range(-pitchVariance_, pitchVariance_);
                tickAudioSource_.PlayOneShot(tickAudioClip_);
            }

            dialogueText_.text = text.Substring(0, subStringLength_);
            timer_ = 0.0f;
        }
    }
}
