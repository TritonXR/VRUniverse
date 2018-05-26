Warnings:
- UniverseSystem reads all JSON files in the VRClubUniverse_Data folder. They must be in the format YEAR.json.
- When using ExecutableSwitch, the user must already have SteamVR imported and did not change the hierarchy of the CameraRig.
- VR Club Universe only works with HTC Vive and Unity projects.


Example JSON File: 2021.json

{
    "PlanetJSON":
    [
		{
		    "Name": "Name of the Project",
			"Creator": "People who created this project",
			"Description": "Describe this project",
			"Year": 2015,
			"Tags": [
				"HTC Vive",
				"Educational"
			],
			"Image": "image.jpg",
			"Executable": "executable.exe"
		},
		{
		    "Name": "Underwater Rings",
			"Creator": "Kristin Agcaoili",
			"Description": "Users have the opportunity to fly underwater and swim through rings like a racetrack in the ocean. Try to teleport through the ocean as fast as you can!",
			"Year": 2015,
			"Tags": [
				"Underwater",
				"HTC Vive",
				"Racing"
			],
			"Image": "image_underwaterrings.jpg",
			"Executable": "executable.exe"
		},
		{
		    "Name": "Nature",
			"Creator": "Kristin Agcaoili",
			"Description": "Nature is a relaxing experience that takes users up into the mountains surrounded by forests and grass. It is a wonderful place to relax and take a break from life's business.",
			"Year": 2015,
			"Tags": [
				"Relaxation",
				"HTC Vive",
				"Nature"
			],
			"Image": "image_nature.jpg",
			"Executable": "executable.exe"
		}
	]
}