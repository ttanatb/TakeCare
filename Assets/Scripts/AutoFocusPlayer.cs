using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class AutoFocusPlayer : MonoBehaviour
{
    PostProcessVolume volume;

    [SerializeField]
    Transform playerTransform;

    [SerializeField]
    float maxBound = 1.1f;
    [SerializeField]
    float minBound = 0.6f;

    [SerializeField]
    float scalingFactor = 1.0f;
    [SerializeField]
    float offset = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        volume = GetComponent<PostProcessVolume>();
    }

    // Update is called once per frame
    void Update()
    {
        // ONLY WORKS IF CAMERA ISN'T ROTATED ALONG Y-AXIS

        float distAlongZ = playerTransform.position.z - transform.position.z;
        //Vector3.Dot(playerTransform.position - transform.position, Vector3.forward);
        //Debug.Log("Dist Along Z: " + distAlongZ);
        distAlongZ = Mathf.Clamp(distAlongZ, minBound, maxBound);
        float noramlizedDist = (distAlongZ - minBound) / (maxBound - minBound); // normalize

        float dist = offset + noramlizedDist * scalingFactor;
        //Debug.Log(dist);

        DepthOfField dof;
        volume.profile.TryGetSettings(out dof);
        dof.focusDistance.value = dist;
    }
}
