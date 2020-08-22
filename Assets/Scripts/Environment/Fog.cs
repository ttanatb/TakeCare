using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fog : MonoBehaviour
{
    Vector3 anchorPos_;

    [SerializeField]
    float verticalMovement_ = 0.1f;
    [SerializeField]
    float verticalMovementVariance_ = 0.05f;

    [SerializeField]
    float movementInterval_ = 0.5f;
    [SerializeField]
    float movementIntervalVariance_ = 0.35f;

    float startingOffset_ = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        anchorPos_ = transform.position;
        verticalMovement_ += Random.Range(-verticalMovementVariance_, verticalMovementVariance_);
        if (Random.value < 0.5f)
            verticalMovement_ = -verticalMovement_;

        movementInterval_ += Random.Range(-movementIntervalVariance_, movementIntervalVariance_);

        startingOffset_ = Random.Range(0.0f, Mathf.PI * 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = anchorPos_.y + Mathf.Sin(Time.time * movementInterval_ + startingOffset_) * verticalMovement_;

        transform.position = pos;
    }
}
