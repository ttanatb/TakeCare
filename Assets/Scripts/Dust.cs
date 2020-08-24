using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{

    SpriteRenderer renderer_;

    [SerializeField]
    Vector2 minStretch = new Vector2(40.0f, 40.0f);

    [SerializeField]
    Vector2 maxStretch = new Vector2(60.0f, 60.0f);

    Vector2 offset_ = Vector2.one;

    [SerializeField]
    Vector2 lerpFactor = Vector2.one;


    [SerializeField]
    float totalRotation_ = 3f;
    [SerializeField]
    float rotationSpeed_ = 0.5f;

    Quaternion anchorRot_;

    // Start is called before the first frame update
    void Start()
    {
        renderer_ = GetComponent<SpriteRenderer>();

        offset_.x = Random.Range(0.0f, Mathf.PI * 2.0f);
        offset_.y = Random.Range(0.0f, Mathf.PI * 2.0f);
        anchorRot_ = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 newSize = Vector2.zero;
        newSize.x = Mathf.Lerp(minStretch.x, maxStretch.x,
            Mathf.Abs(Mathf.Sin(Time.time * lerpFactor.x + offset_.x)));
        newSize.y = Mathf.Lerp(minStretch.y, maxStretch.y,
            Mathf.Abs(Mathf.Cos(Time.time * lerpFactor.y + offset_.y)));
        renderer_.size = newSize;


        Quaternion rot = anchorRot_ *
            Quaternion.AngleAxis(Mathf.Sin(Time.time * rotationSpeed_) * totalRotation_,
            transform.forward);
        transform.rotation = rot;
    }
}
