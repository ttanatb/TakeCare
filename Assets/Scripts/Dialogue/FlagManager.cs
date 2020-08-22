using System;
using System.Collections;
using System.Collections.Generic;
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
        TutorialCompleted = 0,
        SelectedPotato,
        SelectedPineapple,
    }

    FlagCompletionEvent flagCompletedEvent = new FlagCompletionEvent();

    public void AddListener(UnityAction<EventFlag> eventCompletedCb)
    {
        flagCompletedEvent.AddListener(eventCompletedCb);
    }

    public void RemoveListener(UnityAction<EventFlag> eventCompletedCb)
    {
        flagCompletedEvent.RemoveListener(eventCompletedCb);
    }

    [SerializeField]
    HashSet<EventFlag> completedFlags_ = new HashSet<EventFlag>();

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


}
