using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutableSwitch : MonoBehaviour

{
    private static ExecutableSwitch switcher = null;

    public string defaultDatapath = "VRUniverse.exe";

    private char[] delimiterList = { '/', '\\', '.' };
    private bool isLoading;

    private void Awake()
    {
        if (switcher == null) switcher = this;
        else
        {
            Destroy(gameObject);
        }
        isLoading = false;

        DontDestroyOnLoad(gameObject);
    }

    void OnDestroy()
    {
        if(switcher == this) switcher = null;
    }

    public void LoadExecutable(string datapath)
    {
        string[] splitDatapath = datapath.Split(delimiterList);
        string processName = splitDatapath[splitDatapath.Length - 2]; //should extract the process name

        LoadExecutable(datapath, processName);
    }

    public void LoadExecutable(string datapath, string processName)
    {
        if (isLoading) return;
        else isLoading = true;
        if (datapath == null || datapath.Equals("")) datapath = defaultDatapath;

        System.Diagnostics.Process childProcess = System.Diagnostics.Process.Start(Application.dataPath + datapath);
        System.Diagnostics.Process rebootProcess = System.Diagnostics.Process.Start(Application.dataPath + "/../ProcessRebooter.exe", "VRUniverse.exe " + processName);

        Application.Quit();
    }

    public void LoadExecutable()
    {
        LoadExecutable(defaultDatapath);
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
}