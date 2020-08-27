using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator animator;
    Vector3 prevPos;

    [SerializeField]
    float animThresh = 0.1f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        prevPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //bool buttonHori = Input.GetButton("Horizontal");
        //bool buttonVert = Input.GetButton("Vertical");

        //bool left = buttonHori ? Input.GetAxis("Horizontal") < 0.0f : false;
        //bool right = buttonHori ? Input.GetAxis("Horizontal") > 0.0f : false;
        //bool back = buttonVert ? Input.GetAxis("Vertical") > 0.0f : false;
        //bool front = buttonVert ? Input.GetAxis("Vertical") < 0.0f : false;

        Vector3 diff = transform.position - prevPos;

        animator.SetBool("left", diff.x < -animThresh);
        animator.SetBool("right", diff.x > animThresh);
        animator.SetBool("back", diff.z > animThresh);
        animator.SetBool("front", diff.z < -animThresh);

        prevPos = transform.position;
    }
}
