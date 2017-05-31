Quick Start Guide:
Step 1) Make sure you have the SteamVR plugin package in your project.
	Step 1A) If you didn't import the entire package, make sure you at
		 least have the "SteamVR_LoadLevel.cs" script and its dependencies.

Step 2) Import the "VRUniverseExecutableSwitch" unity package into your project.
	Step 2A) You can do this by going to Assets > Import Package > Custom Package...
		 and then selecting the aforementioned package.
	Step 2B) Make sure that both the "ExecutableSwitch" prefab and script are
		 selected.
	Step 2C) Click "Import".

Step 3) Add the "ExecutableSwitch" prefab into your starting scene (the scene that
	the player sees when your application starts up).
	Step 3A) The prefab is set to no be destroyed on level load, so you don't
		 need to add it to every scene.
	Step 3B) The "ExecutableSwitch.cs" script can be added to a GameObject if
		 you don't want to use the prefab. In this case, make sure that
		 there is only one copy of the script (If there are multiples, the
		 GameObjects with the extra copies will be destroyed!), and that the
		 script has a reference to a SteamVR_LoadLevel script.

Step 4) Replace all occurances of Application.Quit() with a call to the ExecutableSwitch
	script's LoadExecutable() function.
	Step 4A) If you exit your application with a function other than Application.Quit(),
		 make sure those functions are replaced as well.
	Step 4B) There are a number of ways to call this function. The most straightforward way
		 is simply: "ExecutableSwitch.LoadExe();".
	Step 4C) You can also give your scripts a reference to the ExecutableSwitch script, or you
		 can make use of the fact that the ExecutableSwitch follows a singleton pattern.
		 Look at the C# file for more details.

Step 5) Enable the "Exit By Menu Button" checkbox if you're using the default SteamVR "[CameraRig]"
	prefab and you don't have a exit button set up yet.
	Step 5A) Do not modify the name of any object in the "[CameraRig]" prefab, or the
		 ExecutableSwitch script will be unable to find the controllers.
	Step 5B) If you have anything mapped to the menu button above the trackpad on either
		 controller, do not enable this function. It will exit the application regardless
		 of any other functions executing at the same time.


More Information on the ExecutableSwitch script:
> The defaultDatapth variable (defaults to "../../../../../VRClubUniverse.exe") should generally
  not be modified unless a VR Club Universe project team member tells you to do so.
> There are four functions for loading an executable with the ExecutableSwitch script.
	1) void LoadExecutable(string datapath):
		This loads the executable at the relative path specified by "datapath". If datapath
		is empty or null, it uses the default datapath instead, which loads the VR Universe
		Executable.
	2) void LoadExecutable():
		This calls the previous LoadExecutable function with the default datapath as the
		input parameter.
	3) static void LoadExe():
		This calls the LoadExecutable() function with the default datapath of the current
		ExecutableSwitch instance.
	4) static void LoadExe(string datapath):
		This calls the LoadExecutable(string datapath) function with the passed datapath
		of the current ExecutableSwitch instance.
> If the VRClubUniverse executable can't be found, the ExecutableSwitch script will still exit the
  application.
> Enabling the "Exit By Menu Button" option is not recommended. It is reliant on the SteamVR camera rig
  being present and unmodified, and it will conflict with any functions tied to the menu button. Only use
  this option if you do not intend to set up advanced input.