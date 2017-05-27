using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecutableSwitch : MonoBehaviour

{
    private static ExecutableSwitch switcher = null;

    public SteamVR_LoadLevel loadlevelscript;
    public string defaultDatapath = "../../../../../VRClubUniverse.exe";


    void Awake()
    {
        if (switcher == null) switcher = this;
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    public void LoadExecutable(string datapath)
    {
        if (loadlevelscript == null ) return;
        if (datapath == null || datapath.Equals("")) datapath = defaultDatapath;

        loadlevelscript.levelName = datapath;
        loadlevelscript.internalProcessPath = Application.dataPath + datapath;

        loadlevelscript.Trigger();
    }

    public void LoadExecutable()
    {
        LoadExecutable(defaultDatapath);
    }

    void OnDestroy()
    {
        if(switcher == this) switcher = null;
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