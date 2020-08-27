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


    [SerializeField]
    private PlayerTalkCoordinator playerTalkCoordinator = null;

    AudioSource audio_;

    [SerializeField]
    AudioClip steppingSfx_;

    [SerializeField]
    float basePitch = 1.0f;

    [SerializeField]
    float pitchVariance = 0.05f;

    [SerializeField]
    Camera activeCamera_ = null;

    [SerializeField]
    bool isWeb = true;

    [SerializeField]
    LayerMask collisionLayerMask = 1;

    public Camera ActiveCamera
    {
        set { activeCamera_ = value; }
        get { return activeCamera_; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (activeCamera_ == null)
            activeCamera_ = Camera.main;

        if (playerTalkCoordinator == null)
            playerTalkCoordinator = GetComponent<PlayerTalkCoordinator>();

        audio_ = GetComponent<AudioSource>();

        isWeb = Application.platform != RuntimePlatform.WebGLPlayer;

        if (isWeb)
            Input.simulateMouseWithTouches = true;
    }

    // Update is called once per frame
    void Update()
    {
        UpdatePositionFromInput();
    }

    private void UpdatePositionFromInput()
    {
        // Disable movement if in dialogue.
        if (playerTalkCoordinator.State == PlayerTalkCoordinator.PlayerDialogueState.InDialogue)
            return;


        float speed = movementSpeed_;
        if (Input.GetKey(KeyCode.LeftShift))
            speed *= dashMultiplier_;

        Vector3 currPosition = transform.position;

        Vector3 movementVec = isWeb ? GetMovementVecWeb() : GetMovementVecPc();
        currPosition += movementVec * speed * Time.deltaTime;

        transform.position = currPosition;
    }

    private Vector3 GetMovementVecWeb()
    {
        Vector3 movementVec = Vector3.zero;
        if (!Input.GetMouseButton(0))
            return movementVec;

        Vector3 mousePos = Input.mousePosition;
        mousePos.x /= Screen.width;
        mousePos.y /= Screen.height;

        Ray ray = activeCamera_.ViewportPointToRay(mousePos);
        RaycastHit hitInfo;
        if (!Physics.Raycast(ray, out hitInfo, float.PositiveInfinity, collisionLayerMask))
            return movementVec;

        movementVec = hitInfo.point - transform.position;
        movementVec.y = 0;
        return movementVec.normalized;
    }

    private Vector3 GetMovementVecPc()
    {
        float horizontalMovement = Input.GetAxis(HORIZONTAL_AXIS);
        float verticalMovement = Input.GetAxis(VERTICAL_AXIS);

        Quaternion rotation = Quaternion.AngleAxis(activeCamera_.transform.eulerAngles.y, Vector3.up);
        Vector3 right = rotation * Vector3.right;
        Vector3 forward = rotation * Vector3.forward;

        return (right * horizontalMovement + forward * verticalMovement).normalized;
    }
}
