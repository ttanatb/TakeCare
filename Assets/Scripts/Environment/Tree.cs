using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tree : MonoBehaviour
{
    Quaternion anchorRot_;

    [SerializeField]
    float totalRotation_ = 3f;
    [SerializeField]
    float totalRotationVariance_ = 1f;

    [SerializeField]
    float rotationSpeed_ = 0.5f;
    [SerializeField]
    float rotationSpeedVariance_ = 0.35f;


    float startingOffset_ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        anchorRot_ = transform.rotation;
        totalRotation_ += Random.Range(-totalRotationVariance_, totalRotationVariance_);
        rotationSpeed_ += Random.Range(-rotationSpeedVariance_, rotationSpeedVariance_);

        startingOffset_ = Random.Range(0.0f, Mathf.PI * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Quaternion rot = anchorRot_ *
            Quaternion.AngleAxis(Mathf.Sin(Time.time * rotationSpeed_ + startingOffset_) * totalRotation_,
            transform.up);
        transform.rotation = rot;
    }
}
