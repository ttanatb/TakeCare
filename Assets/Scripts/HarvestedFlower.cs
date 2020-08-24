using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestedFlower : MonoBehaviour
{
    Renderer[] renderers_;

    [SerializeField]
    Transform start;

    [SerializeField]
    Transform target;

    [SerializeField]
    FlagManager.EventFlag eventFlag;

    FlagManager flagManager_;

    [SerializeField]
    float walkSpeed = 2.0f;

    [SerializeField]
    float rotateSpeed = 2.0f;

    bool isWalking = false;
    float walkProgressTicker = 0.0f;
    Vector3 prevPos = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        flagManager_.AddListener(OnFlagFlipped);
        prevPos = transform.position;

        renderers_ = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in renderers_)
            r.enabled = false;
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlag == flag)
        {
            Debug.Log("Received AnimEventFlag " + flag);
            isWalking = true;

            flagManager_.UnsetFlagCompletion(flag);

            foreach (Renderer r in renderers_)
                r.enabled = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWalking) return;

        walkProgressTicker += walkSpeed * Time.deltaTime;
        transform.position =
            Vector3.Lerp(start.position,
                         target.position,
                         walkProgressTicker);
        transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero,
            walkProgressTicker);

        transform.Rotate(Vector3.forward,
            (transform.position - prevPos).sqrMagnitude * rotateSpeed);

        if (walkProgressTicker >= 1.0f)
        {
            isWalking = false;
            walkProgressTicker = 0.0f;

            foreach (Renderer r in renderers_)
                r.enabled = false;
        }

        prevPos = transform.position;

    }
}
