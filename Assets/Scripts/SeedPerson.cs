using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UIElements;

[Serializable]
public struct EventFlagAnimBoolStringPair
{
    public FlagManager.EventFlag EventFlag;
    public string Sprite;
}

public class SeedPerson : MonoBehaviour
{
    private Animator animator_ = null;

    [SerializeField]
    private EventFlagAnimBoolStringPair[] eventFlagAnimBoolStringPairs_;

    private Dictionary<FlagManager.EventFlag, string> eventFlagToAnimBoolString_;

    FlagManager flagManager_;

    // ASSUMES TWO IN ARRAY
    [SerializeField]
    private Transform[] walkTargets;

    [SerializeField]
    float walkSpeed = 2.0f;

    bool isWalking = false;

    float walkProgressTicker = 0.0f;

    Vector3 prevPos = Vector3.zero;

    string lastFiredAnim_ = "";

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        animator_ = GetComponent<Animator>();
        eventFlagToAnimBoolString_ = new Dictionary<FlagManager.EventFlag, string>();
        foreach (EventFlagAnimBoolStringPair pair in eventFlagAnimBoolStringPairs_)
            eventFlagToAnimBoolString_.Add(pair.EventFlag, pair.Sprite);

        flagManager_.AddListener(OnFlagFlipped);

        prevPos = transform.position;
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToAnimBoolString_.ContainsKey(flag))
        {
            Debug.Log("Received AnimEventFlag " + flag);
            string animString = eventFlagToAnimBoolString_[flag];
            lastFiredAnim_ = animString;
            animator_.SetBool(animString, true);
            flagManager_.UnsetFlagCompletion(flag);
            isWalking = true;
        }
    }


    private void Update()
    {
        if (!isWalking) return;

        walkProgressTicker += walkSpeed * Time.deltaTime;
        transform.position =
            Vector3.Lerp(walkTargets[0].position,
                         walkTargets[1].position,
                         walkProgressTicker);
        transform.forward =
            Vector3.Lerp(walkTargets[0].forward,
                         walkTargets[1].forward,
                         walkProgressTicker);

        if (walkProgressTicker >= 1.0f)
        {
            Transform temp = walkTargets[0];
            walkTargets[0] = walkTargets[1];
            walkTargets[1] = temp;
            isWalking = false;
            animator_.SetBool(lastFiredAnim_, false);
            walkProgressTicker = 0.0f;
        }

        prevPos = transform.position;
    }
}
