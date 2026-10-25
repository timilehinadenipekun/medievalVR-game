
using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLookController : MonoBehaviour,
    IPointerDownHandler, IDragHandler
{
    public Transform player;
    public Transform playerCamera;
    public float sensitivity = 0.15f;

    private float pitch;

    void Start()
    {
        pitch = playerCamera.localEulerAngles.x;

        if (pitch > 180f)
            pitch -= 360f;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Dragging begins.
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 delta = eventData.delta;

        // Turn the player's body left or right.
        player.Rotate(Vector3.up * delta.x * sensitivity);

        // Look up or down without flipping the camera.
        pitch -= delta.y * sensitivity;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        playerCamera.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}
