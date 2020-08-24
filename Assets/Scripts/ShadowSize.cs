using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct EventFlagVector3Pair
{
    public FlagManager.EventFlag EventFlag;
    public Vector3 Vector3;
}

public class ShadowSize : MonoBehaviour
{
    [SerializeField]
    private EventFlagVector3Pair[] eventFlagAnimPairs_;

    private Dictionary<FlagManager.EventFlag, Vector3> eventFlagToAnimStrings_;

    FlagManager flagManager_;


    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        eventFlagToAnimStrings_ = new Dictionary<FlagManager.EventFlag, Vector3>();
        foreach (EventFlagVector3Pair pair in eventFlagAnimPairs_)
            eventFlagToAnimStrings_.Add(pair.EventFlag, pair.Vector3);

        flagManager_.AddListener(OnFlagFlipped);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToAnimStrings_.ContainsKey(flag))
        {
            Debug.Log("Received AnimEventFlag " + flag);
            transform.localScale = eventFlagToAnimStrings_[flag];

            flagManager_.UnsetFlagCompletion(flag);
        }
    }
}
