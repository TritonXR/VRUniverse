using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutableSwitch : MonoBehaviour

{
    private static ExecutableSwitch switcher = null;

    [SerializeField] private string defaultDatapath = "VRUniverse.exe";
    [SerializeField] private int delayTime = 500;

    private char[] delimiterList = { '/', '\\'};
    private bool isLoading;
	private string appDatapath;

    private void Awake()
    {
        if (switcher == null) switcher = this;
        else
        {
            Destroy(this);
        }
        isLoading = false;

		appDatapath = Application.dataPath;

		Debug.Log("appDatapath: " + appDatapath);
    }

    void OnDestroy()
    {
        if(switcher == this) switcher = null;
    }

   public void LoadExecutable(string datapath)
    {
        if (isLoading) return;
        else isLoading = true;

		if (datapath == null || datapath.Equals("")) datapath = defaultDatapath;
		string helperDatapath = Application.dataPath + "/../VRUniverse_Helper.exe";

		Debug.Log("Starting: " + helperDatapath);

		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.FileName = helperDatapath;
		startInfo.Arguments = "\"" + datapath + "\" \"" + appDatapath + "/../VRUniverse.exe\" " + delayTime;

		Debug.Log("Arguments: " + startInfo.Arguments);

		System.Diagnostics.Process rebootProcess = System.Diagnostics.Process.Start(startInfo);

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

    public static string GetFullPath(string filename, string foldername, string year)
    {
#if UNITY_EDITOR
        return Application.dataPath + @"/../Website/data/VRClubUniverseData/VR_Demos/" + year + "/" + foldername + "/" + filename;
#elif UNITY_STANDALONE
        return Application.dataPath + @"/../VRClubUniverse_Data/VR_Demos/" + year + @"/" + foldername + @"/" + filename;
#endif
    }
}