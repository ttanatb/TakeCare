using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct EventFlagCameraMovementPair
{
    public FlagManager.EventFlag EventFlag;
    public PositionAndSize PosAndZoom;
}

[Serializable]
public struct PositionAndSize
{
    public Vector3 Position;
    public float Size;
}

public class CameraZoomController : MonoBehaviour
{
    [SerializeField]
    private EventFlagCameraMovementPair[] eventFlagAnimPairs_;

    private Dictionary<FlagManager.EventFlag, PositionAndSize> eventFlagToCamMovement_;

    FlagManager flagManager_;

    Camera camera_;

    Vector3 targetPos = -Vector3.forward;
    float posLerpFactor = 0.2f;

    float targetSize = 4.5f;
    float sizeLerpFactor = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        eventFlagToCamMovement_ = new Dictionary<FlagManager.EventFlag, PositionAndSize>();
        foreach (EventFlagCameraMovementPair pair in eventFlagAnimPairs_)
        {
            eventFlagToCamMovement_.Add(pair.EventFlag, pair.PosAndZoom);
        }

        camera_ = GetComponent<Camera>();

        flagManager_ = FlagManager.Instance;
        flagManager_.AddListener(OnFlagFlipped);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToCamMovement_.ContainsKey(flag))
        {
            Debug.Log("Moving camera for " + flag);
            flagManager_.UnsetFlagCompletion(flag);

            targetPos = eventFlagToCamMovement_[flag].Position;
            targetSize = eventFlagToCamMovement_[flag].Size;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition =
            Vector3.Lerp(transform.localPosition, targetPos, posLerpFactor);
        camera_.orthographicSize =
            Mathf.Lerp(camera_.orthographicSize, targetSize, sizeLerpFactor);
    }
}
