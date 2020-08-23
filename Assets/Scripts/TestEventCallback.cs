using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TestEventCallback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateText(FlagManager.EventFlag flag)
    {
        GetComponent<TextMeshPro>().text = "Completed: " + flag;
    }
}
