using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class FlagCompletionEvent : UnityEvent<FlagManager.EventFlag>
{
}

public class FlagManager : Singleton<FlagManager>
{
    public enum EventFlag
    {
        Invalid = 0,
        TutHellHoundSit = 1,
        TutHellHoundSleep = 2,
        TutHellHoundShock = 3,
        StartingSeq = 4,
        StartingSeq1 = 5,
        StartingSeq1Huh = 6,
        StartingSeq1Cat = 7,
        StartingSeq2 = 8,
        StartingSeq2Ew = 9,
        StartingSeq2Crops = 10,
        StartingSeq2NoQ = 11,
        StartingSeq2Repeat = 12,
        StartingSeq2Pet = 13,
        StartingSeq2PetBackAway = 14,
        StartingSeq2PetHead = 15,
        StartingSeq2PetTummy = 16,
        CameraReset = 17,
        StartingSeq2PetZoom1 = 18,
        StartingSeq2PetZoom2 = 19,
        StartingSeq2PetZoom3 = 20,
        TOTALCOUNT,
    }

    [SerializeField]
    FlagCompletionEvent flagCompletedEvent = new FlagCompletionEvent();

    public void AddListener(UnityAction<EventFlag> eventCompletedCb)
    {
        flagCompletedEvent.AddListener(eventCompletedCb);
    }

    public void RemoveListener(UnityAction<EventFlag> eventCompletedCb)
    {
        flagCompletedEvent.RemoveListener(eventCompletedCb);
    }

    HashSet<EventFlag> completedFlags_ = new HashSet<EventFlag>();

    [SerializeField]
    EventFlag[] completedFlags_DEBUG;

    [SerializeField]
    EventFlag flagToToggle;

    public void TurnInspectorFlagOn()
    {
        Debug.Log("Turned ON " + flagToToggle);
        completedFlags_.Add(flagToToggle);
        flagCompletedEvent.Invoke(flagToToggle);
    }

    public void TurnInspectorFlagOff()
    {
        completedFlags_.Remove(flagToToggle);
        Debug.Log("Turned OFF " + flagToToggle);
    }

    public bool SetFlagCompletion(EventFlag flag)
    {
        if (completedFlags_.Contains(flag))
        {
            Debug.LogWarning("Flipping Flag " + flag + " eventhough it's already flipped.");
            return false;
        }

        completedFlags_.Add(flag);
        flagCompletedEvent.Invoke(flag);
        return true;
    }

    public bool UnsetFlagCompletion(EventFlag flag)
    {
        if (!completedFlags_.Remove(flag))
        {
            Debug.LogWarning("Unsetting  Flag " + flag + " eventhough it doesn't exist.");
            return false;
        }

        return true;
    }

    public bool SetFlagCompletion(EventFlag[] flags)
    {
        foreach (EventFlag f in flags)
        {
            if (completedFlags_.Contains(f))
            {
                Debug.LogWarning("Flipping Flag " + f + " eventhough it's already flipped.");
            }

            completedFlags_.Add(f);
            flagCompletedEvent.Invoke(f);
        }
        return true;
    }

    public bool GetFlagCompletion(EventFlag flag)
    {
        return completedFlags_.Contains(flag);
    }

    public bool GetFlagCompletion(EventFlag[] flags)
    {
        foreach (EventFlag f in flags)
        {
            if (!completedFlags_.Contains(f))
                return false;
        }

        return true;
    }

    public bool GetFlagUnmet(EventFlag[] flags)
    {
        foreach (EventFlag f in flags)
        {
            if (completedFlags_.Contains(f))
                return false;
        }

        return true;
    }

    private void Start()
    {
        completedFlags_DEBUG = new EventFlag[(int)EventFlag.TOTALCOUNT];
    }

    private void Update()
    {
        if (Debug.isDebugBuild || Application.isEditor)
        {
            completedFlags_.CopyTo(completedFlags_DEBUG);

            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKeyDown(KeyCode.D))
            {
                StringBuilder stringBuilder = new StringBuilder();
                foreach (EventFlag f in completedFlags_)
                {
                    stringBuilder.Append(f + ", ");
                }
                Debug.Log(stringBuilder.ToString());
            }
        }
    }

}
