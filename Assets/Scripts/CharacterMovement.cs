using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    const string HORIZONTAL_AXIS = "Horizontal";
    const string VERTICAL_AXIS = "Vertical";

    [SerializeField]
    private float movementSpeed_ = 5;

    [SerializeField]
    private float dashMultiplier_ = 1.5f;

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
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositionFromInput();
    }

    private void UpdatePositionFromInput()
    {
        // Get Axes
        float horizontalMovement = Input.GetAxis(HORIZONTAL_AXIS);
        float verticalMovement = Input.GetAxis(VERTICAL_AXIS);

        // Adjust speed
        float speed = movementSpeed_;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= dashMultiplier_;

        Vector3 currPosition = transform.position;

        Quaternion rotation = Quaternion.AngleAxis(cameraTransform_.eulerAngles.y, Vector3.up);
        Vector3 right = rotation * Vector3.right;
        Vector3 forward = rotation * Vector3.forward;


        currPosition += right * horizontalMovement * speed * Time.deltaTime;
        currPosition += forward * verticalMovement * speed * Time.deltaTime;

        transform.position = currPosition;
    }
}
