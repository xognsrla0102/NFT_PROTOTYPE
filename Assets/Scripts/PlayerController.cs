using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public VariableJoystick moveJoystick;
    public VariableJoystick rotationJoystick;

    private CharacterController cc;

    [SerializeField] private Transform playerCamera;
    [SerializeField] float gravity;
    [SerializeField] private float mouseSensitivity;
    [SerializeField] private float moveSpd;

    private float cameraRot;
    private float downVelocity;

    [SerializeField] private AnimationCurve jumpFallCurve;
    [SerializeField] private float jumpMultiplier;
    private bool isJump;

    private void Start()
    {
        cc = GetComponent<CharacterController>();

        if (GameManager.Instance.isAndroidMode == false)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void FixedUpdate()
    {
        #region 이동
        Vector2 moveDir;
        if (GameManager.Instance.isAndroidMode)
        {
            moveDir = new Vector2(moveJoystick.Horizontal, moveJoystick.Vertical);
        }
        else
        {
            moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        }
        moveDir.Normalize();

        if (cc.isGrounded) downVelocity = 0f;
        downVelocity += gravity * Time.fixedDeltaTime;

        Vector3 moveVelocity = transform.forward * moveDir.y + transform.right * moveDir.x;
        moveVelocity *= Input.GetKey(KeyCode.LeftShift) ? 1.5f : 1;
        Vector3 gravityVelocity = Vector3.up * downVelocity;
        Vector3 velocity = moveVelocity * moveSpd + gravityVelocity;
        cc.Move(velocity * Time.deltaTime);
        #endregion

        #region 점프
        if (Input.GetKeyDown(KeyCode.Space) && !isJump)
            StartCoroutine(JumpCoroutine());
        #endregion
    }

    public void Update()
    {
        #region 카메라 회전
        if (GameManager.Instance.isAndroidMode) return;

        cameraRot -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        cameraRot = Mathf.Clamp(cameraRot, -90f, 90f);
        playerCamera.localEulerAngles = Vector3.right * cameraRot;

        transform.Rotate(Vector3.up * Input.GetAxis("Mouse X") * mouseSensitivity);
        #endregion
    }

    IEnumerator JumpCoroutine()
    {
        isJump = true;
        float timeInAir = 0f;
        do
        {
            float jumpForce = jumpFallCurve.Evaluate(timeInAir);
            timeInAir += Time.deltaTime;
            cc.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            yield return null;
        } while (!cc.isGrounded && cc.collisionFlags != CollisionFlags.Above);
        isJump = false;
    }
}
