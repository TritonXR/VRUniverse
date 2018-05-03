using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    private static TutorialController instance;

    [SerializeField]
    private GameObject welcomeTutorial_1; // headset. disappear when specific action is completed
    [SerializeField]
    private GameObject yearTutorial_2;
    [SerializeField]
    private GameObject categoryTutorial_3;
    [SerializeField]
    private GameObject searchTutorial_4;
    [SerializeField]
    private GameObject exit_5; // be there forever
    [SerializeField]
    private GameObject lever_6; // be there forever

    int tutorialsFinished;

    const float FP_TOLERANCE = 0.001f;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
    }

    // Use this for initialization
    void Start () {
        tutorialsFinished = 0;
        welcomeTutorial_1.SetActive(true);
        yearTutorial_2.SetActive(false);
        categoryTutorial_3.SetActive(false);
        searchTutorial_4.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		if(tutorialsFinished == 0 && Mathf.Abs(OrbitManager.GetOrbitManager().LinearSpeed) < FP_TOLERANCE)
        {
            Debug.Log("Finished first tutorial");
            tutorialsFinished++;
            welcomeTutorial_1.SetActive(false);
            yearTutorial_2.SetActive(true);
        }

        if (tutorialsFinished == 1 && !UniverseSystem.GetInstance().GetCurrentYear().Equals(UniverseSystem.LOBBY_YEAR_STRING))
        {
            Debug.Log("Finished second tutorial");
            tutorialsFinished++;

            yearTutorial_2.SetActive(false);
            categoryTutorial_3.SetActive(true);
        }

        if(tutorialsFinished == 2 && CategoryManager.GetInstance().GetNumSelected() > 0)
        {
            Debug.Log("Finished third tutorial");
            tutorialsFinished++;

            categoryTutorial_3.SetActive(false);
            searchTutorial_4.SetActive(true);
        }

	}

    public void SkipTutorials()
    {
        Debug.Log("Skipping Tutorials");
        tutorialsFinished = 4;
        welcomeTutorial_1.SetActive(false);
    }

    public static TutorialController GetInstance()
    {
        return instance;
    }
}
