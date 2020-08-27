using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public struct EventFlagImagePair
{
    public FlagManager.EventFlag EventFlag;
    public Sprite Image;
}


public class DialogueImageController : MonoBehaviour
{
    [SerializeField]
    FlagManager.EventFlag showImagePanel_;

    [SerializeField]
    FlagManager.EventFlag hideImagePanel_;

    [SerializeField]
    private EventFlagImagePair[] eventFlagAnimBoolStringPairs_;

    private Dictionary<FlagManager.EventFlag, Sprite> eventFlagToAnimBoolString_;

    FlagManager flagManager_;

    [SerializeField]
    Image[] renderers_;

    [SerializeField]
    Image avatarImage_;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        flagManager_.AddListener(OnFlagFlipped);

        SetImagePanelShowOrHide(false);

        eventFlagToAnimBoolString_ = new Dictionary<FlagManager.EventFlag, Sprite>();
        foreach (EventFlagImagePair pair in eventFlagAnimBoolStringPairs_)
            eventFlagToAnimBoolString_.Add(pair.EventFlag, pair.Image);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (flag == showImagePanel_)
        {
            SetImagePanelShowOrHide(true);
            flagManager_.UnsetFlagCompletion(flag);
        }
        else if (flag == hideImagePanel_)
        {
            SetImagePanelShowOrHide(false);
            flagManager_.UnsetFlagCompletion(flag);
        }
        else if (eventFlagToAnimBoolString_.ContainsKey(flag))
        {
            avatarImage_.sprite = eventFlagToAnimBoolString_[flag];
            flagManager_.UnsetFlagCompletion(flag);
        }
    }

    private void SetImagePanelShowOrHide(bool shouldShow)
    {
        foreach (Image r in renderers_)
            r.enabled = shouldShow;
    }
}
