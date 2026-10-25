
using System.Collections;
using Google.XR.Cardboard;
using UnityEngine;
using UnityEngine.XR.Management;

public class MedievalVRController : MonoBehaviour
{
    public Camera playerCamera;
    public Behaviour touchLookController;
    public Behaviour trackedPoseDriver;
    public GameObject mobileControls;

    public MobilePlayerController mobilePlayerController;
    public VRMovementController vrMovementController;

    private float defaultFOV;
    private bool isVR;
    private bool isSwitching;

    void Start()
    {

        mobilePlayerController.enabled = true;
        vrMovementController.vrModeActive = false;

        defaultFOV = playerCamera.fieldOfView;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        mobilePlayerController.enabled = false;
        vrMovementController.vrModeActive = true;

        // Start in normal mobile mode.
        touchLookController.enabled = true;
        trackedPoseDriver.enabled = false;
        mobileControls.SetActive(true);
    }

    void Update()
    {
        if (!isVR) return;

        if (Api.IsCloseButtonPressed)
        {
            ExitVR();
        }

        if (Api.IsGearButtonPressed)
        {
            Api.ScanDeviceParams();
        }

        Api.UpdateScreenParams();
    }

    // Called by our Enter VR button.
    public void EnterVR()
    {
        if (isVR || isSwitching) return;

        StartCoroutine(StartVR());
    }

    private IEnumerator StartVR()
    {
        isSwitching = true;

        var manager = XRGeneralSettings.Instance.Manager;

        if (!manager.isInitializationComplete)
        {
            yield return manager.InitializeLoader();
        }

        if (manager.activeLoader == null)
        {
            Debug.LogError("Cardboard XR initialization failed.");
            isSwitching = false;
            yield break;
        }

        if (!Api.HasDeviceParams())
        {
            Api.ScanDeviceParams();
        }
        else if (Api.HasNewDeviceParams())
        {
            Api.ReloadDeviceParams();
        }

        manager.StartSubsystems();

        touchLookController.enabled = false;
        trackedPoseDriver.enabled = true;
        mobileControls.SetActive(false);
        mobilePlayerController.enabled = false;
        vrMovementController.vrModeActive = true;
        isVR = true;
        isSwitching = false;

        Debug.Log("Medieval Village VR enabled!");
    }

    public void ExitVR()
    {
        if (!isVR || isSwitching) return;

        isSwitching = true;

        var manager = XRGeneralSettings.Instance.Manager;

        manager.StopSubsystems();
        manager.DeinitializeLoader();

        trackedPoseDriver.enabled = false;
        playerCamera.transform.localRotation =
            Quaternion.identity;

        playerCamera.ResetAspect();
        playerCamera.fieldOfView = defaultFOV;


        vrMovementController.vrModeActive = false;
        mobilePlayerController.enabled = true;


        touchLookController.enabled = true;
        mobileControls.SetActive(true);

        isVR = false;
        isSwitching = false;

        Debug.Log("Returned to mobile mode!");
    }
}
