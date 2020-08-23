using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EventFlagAnimStringPair
{
    public FlagManager.EventFlag EventFlag;
    public string AnimString;
}

public class HellHound : MonoBehaviour
{
    private Animator animator_ = null;

    [SerializeField]
    private EventFlagAnimStringPair[] eventFlagAnimPairs_;

    private Dictionary<FlagManager.EventFlag, string> eventFlagToAnimStrings_;

    FlagManager flagManager_;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        animator_ = GetComponent<Animator>();
        eventFlagToAnimStrings_ = new Dictionary<FlagManager.EventFlag, string>();
        foreach (EventFlagAnimStringPair pair in eventFlagAnimPairs_)
            eventFlagToAnimStrings_.Add(pair.EventFlag, pair.AnimString);


        flagManager_.AddListener(OnFlagFlipped);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToAnimStrings_.ContainsKey(flag))
        {
            Debug.Log("Received AnimEventFlag " + flag);
            animator_.SetTrigger(eventFlagToAnimStrings_[flag]);
            flagManager_.UnsetFlagCompletion(flag);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
