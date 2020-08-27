using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class UIManager : Singleton<UIManager>
{
    FlagManager flagManager_ = null;

    [SerializeField]
    DialogueView dialogueView_ = null;

    [SerializeField]
    DialogueOptionView dialogueOptionView_ = null;


    PlayerTalkCoordinator playerTalkCorodinator_ = null;
    public PlayerTalkCoordinator Player { set { playerTalkCorodinator_ = value; } }

    [SerializeField]
    GameObject mainMenu_;

    [SerializeField]
    bool isWeb = true;

    public bool DisableDialogueInput { set; get; }

    public void ShowMenuScreen(bool v)
    {
        mainMenu_.SetActive(v);
    }

    public void ClearGameState()
    {
        FlagManager.Instance.ClearAllFlags();
        dialogueView_.SetRenderersEnabled(false);
        dialogueView_.Model = null;
        dialogueOptionView_.DialogueOptions = new DialogueOption[0];
    }

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        dialogueView_.SetRenderersEnabled(false);

        isWeb = Application.platform != RuntimePlatform.WebGLPlayer;

        if (isWeb)
            dialogueOptionView_.DisableKeybasedSelection();
    }

    // Update is called once per frame
    void Update()
    {
        CheckInputForMovingSelector();
        CheckInputForConfirm();
    }

    private void CheckInputForMovingSelector()
    {
        if (isWeb || !Input.GetButtonDown("Vertical"))
        {
            return;
        }

        float verticalAxisInput = Input.GetAxis("Vertical");
        if (verticalAxisInput != 0.0f)
        {
            if (dialogueView_.CurrentState != DialogueView.State.WaitingForOptionSelection)
                return;

            if (verticalAxisInput < 0)
            {
                dialogueOptionView_.MoveSelectorDown();
            }

            if (verticalAxisInput > 0)
            {
                dialogueOptionView_.MoveSelectorUp();
            }
            if (!dialogueView_.isActiveAndEnabled ||
                playerTalkCorodinator_.State != PlayerTalkCoordinator.PlayerDialogueState.InDialogue)
                return;

            dialogueView_.SkipToEndOrGoToNext();
        }
    }

    private void CheckInputForConfirm()
    {
        if (!DisableDialogueInput && (isWeb ? IsTalkInputDownWeb() : IsTalkInputDownPC()))
        {
            switch (dialogueView_.CurrentState)
            {
                case DialogueView.State.Scrolling:
                case DialogueView.State.Waiting:
                    if (!dialogueView_.isActiveAndEnabled ||
                         playerTalkCorodinator_.State != PlayerTalkCoordinator.PlayerDialogueState.InDialogue)
                        return;

                    dialogueView_.SkipToEndOrGoToNext();
                    break;
                case DialogueView.State.WaitingForOptionSelection:
                    dialogueOptionView_.ConfirmSelection();
                    break;
                default:
                    break;
            }

        }

    }

    private bool IsTalkInputDownWeb()
    {
        return Input.GetMouseButtonDown(0);
    }

    private bool IsTalkInputDownPC()
    {
        return Input.GetButtonDown("Talk");
    }


    public bool SetAndShowDialogue(DialogueModel dialogueModel, Func<bool> dialogueCompletedCb)
    {
        // No models available.
        if (dialogueModel == null)
            return false;

        dialogueView_.Model = dialogueModel;

        // Create list of dialogue completion callbacks.
        List<Func<bool>> dialogueCompletedCbs = new List<Func<bool>>(2);
        dialogueCompletedCbs.Add(dialogueCompletedCb);
        dialogueCompletedCbs.Add(() =>
        {
            flagManager_.SetFlagCompletion(dialogueModel.FlagsToMarkComplete);
            dialogueView_.SetRenderersEnabled(false);
            return true;
        });
        dialogueView_.DialogueCompletedCbs = dialogueCompletedCbs;

        // Start scrollin'.
        bool result = dialogueView_.StartDialogueScroll();
        if (result)
        {
            dialogueView_.SetRenderersEnabled(true);
        }

        return result;
    }
}
