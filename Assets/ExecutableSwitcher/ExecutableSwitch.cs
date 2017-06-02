using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutableSwitch : MonoBehaviour

{
    private static ExecutableSwitch switcher = null;
    private SteamVR_TrackedController controller_left = null;
    private SteamVR_TrackedController controller_right = null;

    public SteamVR_LoadLevel loadlevelscript;
    public string defaultDatapath = "/../../../../../VRUniverse.exe";
    public bool exitByMenuButton = false;
    public float delayedSearchTime = 0.75f;

    private void Awake()
    {
        if (switcher == null) switcher = this;
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += HandleLevelLoaded;
        SetupControllerListener();
        if (controller_left == null || controller_right == null) StartCoroutine(DelayedSetup());
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= HandleLevelLoaded;
        CleanUpControllerListener();
    }

    void OnDestroy()
    {
        if(switcher == this) switcher = null;
        CleanUpControllerListener();
    }

    public void LoadExecutable(string datapath)
    {
        if (loadlevelscript == null) return;
        if (datapath == null || datapath.Equals("")) datapath = defaultDatapath;
        if (!datapath.StartsWith("/") && !datapath.StartsWith("\\")) datapath = "/" + datapath;

        loadlevelscript.levelName = datapath;

        loadlevelscript.internalProcessPath = Application.dataPath + "/" + datapath;

        loadlevelscript.Trigger();
    }

    public void LoadExecutable()
    {
        LoadExecutable(defaultDatapath);
    }

    private void SetupControllerListener()
    {
        CleanUpControllerListener();
        GameObject l_control = GameObject.Find("[CameraRig]/Controller (left)");
        if (l_control != null)
        {
            controller_left = l_control.GetComponent<SteamVR_TrackedController>();
            if (controller_left == null)
            {
                Debug.LogWarning("Could not find left SteamVR_TrackedController script. Cannot detect exit button press.");
            }
        }
        else
        {
            Debug.LogWarning("Could not find left controller. Cannot detect exit button press.");
            controller_left = null;
        }

        GameObject r_control = GameObject.Find("[CameraRig]/Controller (right)");
        if (r_control != null)
        {
            controller_right = l_control.GetComponent<SteamVR_TrackedController>();
            if (controller_right == null)
            {
                Debug.LogWarning("Could not find right SteamVR_TrackedController script. Cannot detect exit button press.");
            }
        }
        else
        {
            Debug.LogWarning("Could not find right controller. Cannot detect exit button press.");
            controller_right = null;
        }

        if(controller_left != null || controller_right != null)
        {
            if(controller_left != null)
            {
                controller_left.MenuButtonClicked += HandleMenuClicked;
                Debug.Log("Successfully Registered Left Controller");
            }

            if (controller_right != null)
            {
                controller_right.MenuButtonClicked += HandleMenuClicked;
                Debug.Log("Successfully Registered Right Controller");
            }
        }
    }

    private void CleanUpControllerListener()
    {
        if(controller_left != null)
        {
            controller_left.MenuButtonClicked -= HandleMenuClicked;
            controller_left = null;
            Debug.Log("De-registered Left Controller");
        }

        if(controller_right != null)
        {
            controller_right.MenuButtonClicked -= HandleMenuClicked;
            controller_right = null;
            Debug.Log("De-registered Right Controller");
        }
    }

    private void HandleLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        SetupControllerListener();
        if (controller_left == null || controller_right == null) StartCoroutine(DelayedSetup());
    }

    private void HandleMenuClicked(object sender, ClickedEventArgs e)
    {
        Debug.Log("Detected Menu Button Click");
        if (exitByMenuButton) LoadExecutable();
    }

    public static ExecutableSwitch GetExecutableSwitch()
    {
        if (switcher == null) Debug.LogError("ExecutableSwitcher Error: No ExecutableSwitch script exists in this scene!");
        return switcher;
    }

    public static void LoadExe()
    {
        if (switcher == null) Debug.LogError("ExecutableSwitcher Error: No ExecutableSwitch script exists in this scene!");
        else switcher.LoadExecutable();
    }

    public static void LoadExe(string datapath)
    {
        if (switcher == null) Debug.LogError("ExecutableSwitcher Error: No ExecutableSwitch script exists in this scene!");
        else switcher.LoadExecutable(datapath);
    }

    IEnumerator DelayedSetup()
    {
        yield return new WaitForSeconds(delayedSearchTime);
        SetupControllerListener();
    }
}