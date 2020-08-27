using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField]
    Slider bgmVolumeSlider_;

    [SerializeField]
    Slider sfxSlider_;

    [SerializeField]
    GameObject panelToTogle_;

    VolumeManager manager_;
    // Start is called before the first frame update
    void Start()
    {
        manager_ = VolumeManager.Instance;
        bgmVolumeSlider_.value = manager_.BackgroundMusicVolume;
        sfxSlider_.value = manager_.SoundEffectVolume;

        bgmVolumeSlider_.onValueChanged.AddListener(SetBgmVolume);
        sfxSlider_.onValueChanged.AddListener(SetSfxVolume);
    }

    private void SetBgmVolume(float val)
    {
        manager_.BackgroundMusicVolume = val;
    }

    private void SetSfxVolume(float val)
    {
        manager_.SoundEffectVolume = val;
    }

    public void ToggleUI()
    {
        panelToTogle_.SetActive(!panelToTogle_.activeSelf);
    }
}
