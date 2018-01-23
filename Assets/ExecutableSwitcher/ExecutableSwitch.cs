using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExecutableSwitch : MonoBehaviour

{
    private static ExecutableSwitch switcher = null;

    public string defaultDatapath = "VRUniverse.exe";

    private char[] delimiterList = { '/', '\\'};
    private bool isLoading;
	private string appDatapath;

    private void Awake()
    {
        if (switcher == null) switcher = this;
        else
        {
            Destroy(gameObject);
        }
        isLoading = false;

        DontDestroyOnLoad(gameObject);

		appDatapath = Application.dataPath;
		string[] pathComponents = appDatapath.Split(delimiterList);
		appDatapath = "";
		for (int count = 0; count < pathComponents.Length; count++)
		{
			if (pathComponents[count].Contains(" "))
			{
				pathComponents[count] = "\"" + pathComponents[count] + "\"";
			}
			appDatapath += pathComponents[count];
			if (count < pathComponents.Length - 1) appDatapath += "\\";
		}

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
		else datapath = datapath.Replace('/', '\\');
		string helperDatapath = Application.dataPath + "\\..\\VRUniverse_Helper.bat";

		Debug.Log("Starting: " + helperDatapath);

		System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
		startInfo.FileName = helperDatapath;
		startInfo.Arguments = appDatapath + datapath + " " + appDatapath + "\\..\\VRUniverse.exe";

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
}