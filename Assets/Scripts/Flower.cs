using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public struct EventFlagSpritePair
{
    public FlagManager.EventFlag EventFlag;
    public Sprite Sprite;
}

public class Flower : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer renderer_ = null;

    [SerializeField]
    private EventFlagSpritePair[] eventFlagAnimPairs_;

    private Dictionary<FlagManager.EventFlag, Sprite> eventFlagToAnimStrings_;

    FlagManager flagManager_;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        eventFlagToAnimStrings_ = new Dictionary<FlagManager.EventFlag, Sprite>();
        foreach (EventFlagSpritePair pair in eventFlagAnimPairs_)
            eventFlagToAnimStrings_.Add(pair.EventFlag, pair.Sprite);

        flagManager_.AddListener(OnFlagFlipped);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToAnimStrings_.ContainsKey(flag))
        {
            Debug.Log("Received AnimEventFlag " + flag);
            renderer_.sprite = eventFlagToAnimStrings_[flag];
            renderer_.enabled = true;

            flagManager_.UnsetFlagCompletion(flag);
        }
    }
}
