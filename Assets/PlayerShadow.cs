using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField]
    float yPos = 0.01f;

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        pos.y = yPos;
        transform.position = pos;
    }
}
