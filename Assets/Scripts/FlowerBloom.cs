using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct EventFlagRendererSpritePair
{
    public FlagManager.EventFlag EventFlag;
    public RendererAndSprite[] RendererAndSprites;
}

[Serializable]
public struct RendererAndSprite
{
    public SpriteRenderer Renderer;
    public Sprite Sprite;
}

public class FlowerBloom : MonoBehaviour
{
    [SerializeField]
    private EventFlagRendererSpritePair[] eventFlagAnimPairs_;

    private Dictionary<FlagManager.EventFlag, RendererAndSprite[]> eventFlagToAnimStrings_;

    FlagManager flagManager_;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        eventFlagToAnimStrings_ = new Dictionary<FlagManager.EventFlag, RendererAndSprite[]>();
        foreach (EventFlagRendererSpritePair pair in eventFlagAnimPairs_)
        {
            RendererAndSprite[] array = new RendererAndSprite[pair.RendererAndSprites.Length];
            pair.RendererAndSprites.CopyTo(array, 0);

            eventFlagToAnimStrings_.Add(pair.EventFlag, array);
        }


        flagManager_.AddListener(OnFlagFlipped);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToAnimStrings_.ContainsKey(flag))
        {
            Debug.Log("Received AnimEventFlag " + flag);
            foreach (RendererAndSprite rAndS in eventFlagToAnimStrings_[flag])
            {
                SpriteRenderer renderer = rAndS.Renderer;
                renderer.sprite = rAndS.Sprite;
                renderer.enabled = true;
            }

            flagManager_.UnsetFlagCompletion(flag);
        }
    }
}