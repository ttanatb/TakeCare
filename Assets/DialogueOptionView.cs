using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionView : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI[] textOptions_;

    [SerializeField]
    private Button[] optionButtons_;

    [SerializeField]
    private Image[] optionButtonImages_;

    [SerializeField]
    private Color selectedButtonColor_;

    [SerializeField]
    private Color defaultButtonColor_;

    FlagManager flagManager_;
    DialogueOption[] dialogueOptions_;

    public DialogueOption[] DialogueOptions
    {
        set
        {
            selectedIndex = -1;
            availableOptions_ = 0;

            if (value.Length > textOptions_.Length)
            {
                Debug.LogError("Providing more dialogue options than what is supported.");
            }

            for (int i = 0; i < textOptions_.Length; i++)
            {
                if (i >= value.Length ||
                    !flagManager_.GetFlagCompletion(value[i].RequiredFlagsForDialogue))
                {
                    textOptions_[i].enabled = false;
                    optionButtons_[i].enabled = false;
                    optionButtonImages_[i].enabled = false;
                    continue;
                }

                availableOptions_++;

                textOptions_[i].enabled = true;
                optionButtons_[i].enabled = true;
                optionButtonImages_[i].enabled = true;

                textOptions_[i].text = value[i].DialogueText;
                int cachedIndex = i;
                optionButtons_[i].onClick.AddListener(() =>
                {
                    Debug.Log("Selecting button: " + cachedIndex);
                    flagManager_.SetFlagCompletion(dialogueOptions_[cachedIndex].FlagsToMarkAsComplete);

                    if (optionSelectedCb_ == null)
                    {
                        Debug.LogWarning("OptionSelectedCB is null -- How will " +
                            "DialogueView know to proceed to next dialogue?");
                    }
                    else
                    {
                        optionSelectedCb_.Invoke();
                    }
                    HideOptions();

                });

                dialogueOptions_ = value;
            }

        }
    }

    private Func<bool> optionSelectedCb_;
    public Func<bool> OptionSelectedCb
    {
        get { return optionSelectedCb_; }
        set { optionSelectedCb_ = value; }
    }

    int availableOptions_ = 0;
    private int selectedIndex = -1;

    public void MoveSelectorDown()
    {
        int prevIndex = selectedIndex++;

        if (selectedIndex < 0 || selectedIndex >= availableOptions_)
            selectedIndex -= availableOptions_;

        if (!optionButtonImages_[selectedIndex].enabled)
            return;

        if (prevIndex >= 0 && prevIndex < optionButtonImages_.Length)
            optionButtonImages_[prevIndex].color = defaultButtonColor_;

        optionButtonImages_[selectedIndex].color = selectedButtonColor_;

    }

    public void MoveSelectorUp()
    {
        int prevIndex = selectedIndex--;

        if (selectedIndex < 0 || selectedIndex >= availableOptions_)
            selectedIndex += availableOptions_;

        if (!optionButtonImages_[selectedIndex].enabled)
            return;

        if (prevIndex >= 0 && prevIndex < optionButtonImages_.Length)
            optionButtonImages_[prevIndex].color = defaultButtonColor_;

        optionButtonImages_[selectedIndex].color = selectedButtonColor_;
    }

    public void ConfirmSelection()
    {
        if (selectedIndex < 0 || selectedIndex >= availableOptions_)
        {
            // Go to first option as selection.
            selectedIndex = 0;
            optionButtonImages_[selectedIndex].color = selectedButtonColor_;
        }
        else
        {
            // Select that button.
            optionButtonImages_[selectedIndex].color = defaultButtonColor_;
            optionButtons_[selectedIndex].onClick.Invoke();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        for (int i = 0; i < optionButtonImages_.Length; i++)
        {
            optionButtonImages_[i].color = defaultButtonColor_;
        }
        HideOptions();
    }

    private void HideOptions()
    {
        for (int i = 0; i < textOptions_.Length; i++)
        {
            textOptions_[i].enabled = false;
            optionButtons_[i].enabled = false;
            optionButtonImages_[i].enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
