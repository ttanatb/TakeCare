using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct EventFlagEventFlagPair
{
    public FlagManager.EventFlag PreReqEventFlag;
    public FlagManager.EventFlag PreReqUnmetEventFlag;
    public FlagManager.EventFlag EventFlagToFire;
    public bool Fired;
}

public class TriggerPeopleWalkingIn : MonoBehaviour
{
    [SerializeField]
    LayerMask playerLayerMask_;

    [SerializeField]
    private EventFlagEventFlagPair[] eventFlagAnimPairs_;

    FlagManager flagManager_;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayerMask_ == (playerLayerMask_ | (1 << other.gameObject.layer)))
        {
            for (int i = 0; i < eventFlagAnimPairs_.Length; i++)
            {
                FlagManager.EventFlag prereq = eventFlagAnimPairs_[i].PreReqEventFlag;
                FlagManager.EventFlag unmet = eventFlagAnimPairs_[i].PreReqUnmetEventFlag;
                FlagManager.EventFlag flagToFire = eventFlagAnimPairs_[i].EventFlagToFire;
                bool fired = eventFlagAnimPairs_[i].Fired;

                if (flagManager_.GetFlagCompletion(prereq) &&
                    !flagManager_.GetFlagCompletion(unmet) && !fired)
                {
                    flagManager_.SetFlagCompletion(flagToFire);
                    eventFlagAnimPairs_[i].Fired = true;
                }
            }
        }
    }
}
