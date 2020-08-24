using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBillboard : MonoBehaviour
{

    [SerializeField]
    private float scrollSpeed_ = 0.5f;

    [SerializeField]
    private float scrollVariance_ = 0.1f;

    //[SerializeField]
    //Transform cameraTransform_ = null;

    [SerializeField]
    float boundMinX = -15.0f;

    [SerializeField]
    float boundMaxX = 15.0f;

    //public Transform CameraTransform
    //{
    //    set { cameraTransform_ = value; }
    //    get { return cameraTransform_; }
    //}

    // Start is called before the first frame update
    void Start()
    {
        scrollSpeed_ += Random.Range(-scrollVariance_, scrollVariance_);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos += -Vector3.right * scrollSpeed_ * Time.deltaTime;
        if (pos.x <= boundMinX)
            pos.x += boundMaxX - boundMinX;

        transform.position = pos;

        //Quaternion rot = transform.rotation;
        //rot = Quaternion.LookRotation((transform.position - cameraTransform_.position).normalized);
        //transform.SetPositionAndRotation(pos, rot);
    }
}
