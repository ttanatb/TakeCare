using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct EventFlagAudioClipPair
{
    public FlagManager.EventFlag EventFlag;
    public AudioClip AudioClip;
}



public class BgmManager : MonoBehaviour
{
    [SerializeField]
    AudioSource[] audioSources_;
    int audioSourcesIndex = 0;

    AudioSource currAudioSource_;

    AudioSource audioSourceToFadeOut_;
    AudioSource audioSourceToFadeIn_;

    [SerializeField]
    float fadeOutDuration = 1.0f;
    float fadeOutTimer = 0.0f;

    [SerializeField]
    float fadeInDuration = 1.0f;
    float fadeInTimer = 0.0f;

    [SerializeField]
    private EventFlagAudioClipPair[] eventFlagAnimBoolStringPairs_;

    private Dictionary<FlagManager.EventFlag, AudioClip> eventFlagToAnimBoolString_;

    FlagManager flagManager_;
    VolumeManager volumeManager_;


    // Start is called before the first frame update
    void Start()
    {
        flagManager_ = FlagManager.Instance;
        volumeManager_ = VolumeManager.Instance;
        flagManager_.AddListener(OnFlagFlipped);


        eventFlagToAnimBoolString_ = new Dictionary<FlagManager.EventFlag, AudioClip>();
        foreach (EventFlagAudioClipPair pair in eventFlagAnimBoolStringPairs_)
            eventFlagToAnimBoolString_.Add(pair.EventFlag, pair.AudioClip);
    }

    void OnFlagFlipped(FlagManager.EventFlag flag)
    {
        if (eventFlagToAnimBoolString_.ContainsKey(flag))
        {
            FadeTo(eventFlagToAnimBoolString_[flag]);
            flagManager_.UnsetFlagCompletion(flag);
        }
    }

    void FadeTo(AudioClip audioClip)
    {
        if (currAudioSource_ != null)
        {
            audioSourceToFadeOut_ = currAudioSource_;
            fadeOutTimer = 0.0f;
        }

        audioSourcesIndex++;
        if (audioSourcesIndex >= audioSources_.Length)
            audioSourcesIndex -= audioSources_.Length;

        if (audioClip == null)
        {
            currAudioSource_ = null;
        }
        else
        {
            currAudioSource_ = audioSources_[audioSourcesIndex];
            audioSourceToFadeIn_ = currAudioSource_;
            fadeInTimer = 0.0f;
            currAudioSource_.clip = audioClip;
            currAudioSource_.Play();
        }
    }


    // Update is called once per frame
    void Update()
    {
        float maxVolume = volumeManager_.BackgroundMusicVolume;

        if (audioSourceToFadeIn_ != null)
        {
            fadeInTimer += Time.deltaTime;
            audioSourceToFadeIn_.volume = Mathf.Clamp(fadeInTimer / fadeInDuration, 0.0f, maxVolume);

            if (fadeInTimer > fadeInDuration)
            {
                audioSourceToFadeIn_.volume = maxVolume;
                audioSourceToFadeIn_ = null;
            }
        }

        if (audioSourceToFadeOut_ != null)
        {
            fadeOutTimer += Time.deltaTime;
            audioSourceToFadeOut_.volume = Mathf.Lerp(maxVolume, 0.0f,
                Mathf.Clamp(fadeOutTimer / fadeOutDuration, 0.0f, 1.0f));

            if (fadeOutTimer > fadeOutDuration)
            {
                audioSourceToFadeOut_.Stop();
                audioSourceToFadeIn_ = null;
            }
        }

    }
}
