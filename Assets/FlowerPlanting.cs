using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FlowerPlanting : MonoBehaviour
{
    FlagManager flagManager_;
    VolumeManager volumeManager_;

    [SerializeField]
    TextMeshPro interactbleText_ = null;

    [SerializeField]
    FlagManager.EventFlag preReqFlag_;

    [SerializeField]
    FlagManager.EventFlag preReqUnmetFlag_;

    [SerializeField]
    FlagManager.EventFlag flagToFire_;

    [SerializeField]
    AnimationClip animationClip_;

    [SerializeField]
    string animTriggerString_ = "plant";

    Animator animator_;
    AudioSource audioSource_;


    [SerializeField]
    LayerMask playerLayerMask_;

    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        volumeManager_ = VolumeManager.Instance;
        flagManager_.AddListener(OnFlagFlipped);
        SetHasInteraction(false);
        animator_ = GetComponent<Animator>();
        audioSource_ = GetComponent<AudioSource>();
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (preReqFlag_ == flag && !flagManager_.GetFlagCompletion(preReqUnmetFlag_))
        {
            SetHasInteraction(true);
        }
    }

    public void SetHasInteraction(bool isInteractable)
    {
        interactbleText_.gameObject.SetActive(isInteractable);
    }

    public void SetInteractable(bool isInteractable)
    {
        interactbleText_.text = isInteractable ? "!" : "?";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayerMask_ == (playerLayerMask_ | (1 << other.gameObject.layer)))
        {
            SetInteractable(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (playerLayerMask_ == (playerLayerMask_ | (1 << other.gameObject.layer)))
        {
            SetInteractable(false);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerLayerMask_ == (playerLayerMask_ | (1 << other.gameObject.layer)))
        {
            if (Input.GetButtonDown("Talk") &&
                flagManager_.GetFlagCompletion(preReqFlag_) &&
                !flagManager_.GetFlagCompletion(preReqUnmetFlag_) &&
                !flagManager_.GetFlagCompletion(flagToFire_))
            {
                audioSource_.volume = volumeManager_.SoundEffectVolume;
                animator_.SetTrigger(animTriggerString_);
                SetHasInteraction(false);
                Invoke("FlipNextFlag", animationClip_.length + 1);
            }
        }
    }

    public void FlipNextFlag()
    {
        flagManager_.SetFlagCompletion(flagToFire_);
    }
}
