using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class TeleportPoint : MonoBehaviour
{
    [SerializeField]
    Camera associatedCamera_;

    [SerializeField]
    AudioListener associatedAudioListener_;

    [SerializeField]
    Light associatedLight_;

    [SerializeField]
    LayerMask playerLayerMask_ = 9;

    [SerializeField]
    TeleportPoint pointToTeleportTo_;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void SwitchCamAndLights(bool switchToOn)
    {
        associatedCamera_.enabled = switchToOn;
        associatedLight_.enabled = switchToOn;
        associatedAudioListener_.enabled = switchToOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerLayerMask_ == (playerLayerMask_ | (1 << other.gameObject.layer)))
        {
            PlayerTalkCoordinator player = other.GetComponent<PlayerTalkCoordinator>();
            if (player == null)
            {
                Debug.LogError("Entered trigger of chracter who is not NPC.");
                return;
            }

            if (player.TeleportTo(pointToTeleportTo_.transform.position, this, pointToTeleportTo_))
            {
                SwitchCamAndLights(false);
                pointToTeleportTo_.SwitchCamAndLights(true);
            }
        }
    }
}
