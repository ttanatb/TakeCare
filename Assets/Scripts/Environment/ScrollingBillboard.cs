using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBillboard : MonoBehaviour
{
    const float BOUND_X = 15.0f;

    [SerializeField]
    private float scrollSpeed_ = 0.5f;

    [SerializeField]
    private float scrollVariance_ = 0.1f;

    Transform cameraTransform_ = null;

    public Transform CameraTransform
    {
        set { cameraTransform_ = value; }
        get { return cameraTransform_; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (cameraTransform_ == null)
            cameraTransform_ = Camera.main.transform;

        scrollSpeed_ += Random.Range(-scrollVariance_, scrollVariance_);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos += -cameraTransform_.right * scrollSpeed_ * Time.deltaTime;
        if (pos.x <= -BOUND_X)
            pos.x += 2.0f * BOUND_X;

        transform.position = pos;

        Quaternion rot = transform.rotation;
        rot = Quaternion.LookRotation((transform.position - cameraTransform_.position).normalized);
        transform.SetPositionAndRotation(pos, rot);
    }
}
