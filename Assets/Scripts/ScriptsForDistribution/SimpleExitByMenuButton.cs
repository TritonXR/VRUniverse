using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Usage: 
 * > This script closes the application if the Menu button is pressed (the circular button with three lines on it; there's one on
 *   both Vive controllers and one on the left Oculus controller).
 * > Attach this script to the camera rig in your scene and set the ControlMode variable in the inspector to VIVE if you're building for the HTC Vive
 *   or to OCULUS if you're building for the OCULUS RIFT. Though this script will work no matter where it is in the scene hiearchy, performance may be
 *   better if this script is located on the camera rig.
 * > For the Vive, the controllers must have the SteamVR_TrackedController scripts attached. If this script is attached to one of the controllers
 *   (or if only one controller has the SteamVR_TrackedController script attached), then this script will only respond to menu button presses on that
 *   controller.
 */
public class SimpleExitByMenuButton : MonoBehaviour {

    public enum VRSystemInUse { UNSELECTED, VIVE, OCULUS };

    [SerializeField] private VRSystemInUse ControlMode = VRSystemInUse.UNSELECTED;

    private SteamVR_TrackedController[] controllers = null;

	// Use this for initialization
	void Start () {

        // Displays an error if the system hasn't been selected
        if (ControlMode == VRSystemInUse.UNSELECTED)
        {
            Debug.LogError("Control mode is unselected! Please select a control mode!");
        }

        // if the mode has been set to Vive, go find the controllers so button inputs can be monitored
		if (ControlMode == VRSystemInUse.VIVE)
        {
            FindSteamVRTrackedControllers();
        }
	}
	
	// Update is called once per frame
	void Update () {
        // 'quit' is initialized to false, but is set to true if any controller's menu button is pressed
        bool quit = false;

        // checks Vive controllers
		if (ControlMode == VRSystemInUse.VIVE)
        {
            // controllers is only null here if the Control Mode was changed after Start was called
            if (controllers == null)
            {
                FindSteamVRTrackedControllers();
            }

            foreach(SteamVR_TrackedController control in controllers)
            {
                if (control.menuPressed) quit = true;
            }
        }

        // checks Oculus controllers
        if (ControlMode == VRSystemInUse.OCULUS)
        {
            if(OVRInput.Get(OVRInput.RawButton.Start)) quit = true;
        }

        // exit the application if any of the menu buttons have been pressed
        if (quit) Application.Quit();
    }

    // This function switches the control mode.
    // Call this function only if you're detecting which system you're using at runtime.
    public void SetControllerMode(VRSystemInUse system)
    {
        ControlMode = system;
        switch(ControlMode)
        {
            case VRSystemInUse.VIVE:
                FindSteamVRTrackedControllers();
                break;
            case VRSystemInUse.OCULUS:
                break;
            default:
                Debug.LogError("Control mode is unselected or invalid! Please select a valid control mode!");
                break;
        }
    }

    // finds the Vive controller scripts so that their inputs can be monitored
    private void FindSteamVRTrackedControllers()
    {
        // Ideally, this script will be on the camera rig, so the controller scripts will be on child game objects
        controllers = GetComponentsInChildren<SteamVR_TrackedController>(true);

        if (controllers.Length < 1)
        {
            List<SteamVR_TrackedController> temp_controller_list = new List<SteamVR_TrackedController>();

            // if the controllers weren't on any child game objects, search everything (this is really slow)
            foreach (SteamVR_TrackedController potential_controller in Resources.FindObjectsOfTypeAll<SteamVR_TrackedController>() as SteamVR_TrackedController[])
            {
                if (potential_controller.hideFlags == HideFlags.NotEditable || potential_controller.hideFlags == HideFlags.HideAndDontSave)
                    continue;
#if UNITY_EDITOR
                if (!UnityEditor.EditorUtility.IsPersistent(potential_controller.transform.root.gameObject))
                    continue;
#endif

                temp_controller_list.Add(potential_controller);
            }

            controllers = temp_controller_list.ToArray();

            // At this point, if no controllers have been found, then there are likely none in the scene
            if (controllers.Length < 1)
            {
                Debug.LogError("Could not find SteamVR_TrackedController scripts! Make that the controllers have the SteamVR_TrackedController scripts attached!");
            }
        }
    }
}
