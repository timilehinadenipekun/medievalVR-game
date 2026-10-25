
using UnityEngine;
using UnityEngine.UI;

public class FOVController : MonoBehaviour
{
    public Camera playerCamera;
    public Slider fovSlider;

    void Start()
    {
        fovSlider.value = playerCamera.fieldOfView;
        fovSlider.onValueChanged.AddListener(ChangeFOV);
    }

    void ChangeFOV(float newFOV)
    {
        playerCamera.fieldOfView = newFOV;
    }
}
