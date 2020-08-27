using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeManager : Singleton<VolumeManager>
{
    const float DEFAULT_BGM_VOLUME = 0.55f;
    const float DEFAULT_SFX_VOLUME = 0.75f;

    public float BackgroundMusicVolume { get; set; }

    public float SoundEffectVolume { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        BackgroundMusicVolume = DEFAULT_BGM_VOLUME;
        SoundEffectVolume = DEFAULT_SFX_VOLUME;
    }
}
