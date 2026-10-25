
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class VRMovementController : MonoBehaviour
{
    public Transform playerCamera;
    public float walkingSpeed = 2f;
    public float boostMultiplier = 1.75f;
    public float doubleTapInterval = 0.3f;
    public float boostDuration = 2f;

    private CharacterController controller;
    private float lastTapTime = -10f;
    private float boostEndTime;
    private float verticalVelocity;

    // The VR manager will switch this on only in VR mode.
    public bool vrModeActive = false;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!vrModeActive || playerCamera == null)
            return;

        bool holdingScreen = Input.touchCount > 0;

        if (Input.touchCount > 0 &&
            Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (Time.time - lastTapTime <= doubleTapInterval)
                boostEndTime = Time.time + boostDuration;

            lastTapTime = Time.time;
        }

        // Travel in the horizontal direction the player faces.
        Vector3 direction = playerCamera.forward;
        direction.y = 0f;
        direction.Normalize();

        float speed = Time.time < boostEndTime
            ? walkingSpeed * boostMultiplier
            : walkingSpeed;

        if (controller.isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        verticalVelocity += -9.81f * Time.deltaTime;

        Vector3 movement = holdingScreen
            ? direction * speed
            : Vector3.zero;

        movement.y = verticalVelocity;

        controller.Move(movement * Time.deltaTime);
    }
}
