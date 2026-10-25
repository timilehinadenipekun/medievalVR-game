
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MobilePlayerController : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float gravity = -9.81f;
    public VirtualJoystick joystick;
    private CharacterController controller;
    private float verticalVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        // Keep the player on the ground.
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // Use the virtual joystick if it's available and active.
        if (joystick != null && joystick.Direction.sqrMagnitude > 0.01f)
        {
            horizontal = joystick.Direction.x;
            vertical = joystick.Direction.y;
        }
        Vector3 movement =
            transform.right * horizontal +
            transform.forward * vertical;

        movement = Vector3.ClampMagnitude(movement, 1f);

        movement.y = 0;

        controller.Move(
            (movement * moveSpeed +
            Vector3.up * verticalVelocity) * Time.deltaTime
        );
    }
}
