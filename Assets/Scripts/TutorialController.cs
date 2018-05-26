using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialController : MonoBehaviour {

    private static TutorialController instance;

    [SerializeField] private GameObject[] tutorialPanels;
    [SerializeField] private GameObject tutorialSpawnPoint; // spawn point
    


    int tutorialsFinished = 0;

    bool tutorialDismissed = false;

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
        foreach (GameObject tutorial in tutorialPanels) tutorial.SetActive(false);

        if (tutorialsFinished < tutorialPanels.Length)
        {
            tutorialPanels[tutorialsFinished].SetActive(true);
            tutorialPanels[tutorialsFinished].GetComponent<TutorialMove>().StartFollowing();
        }


    }
	
	// Update is called once per frame
	void Update () {
        if (tutorialsFinished == 0 && !tutorialPanels[0].GetComponent<TutorialMove>().IsFollowing())
        {
            AdvanceTutorial();
        }
        if (tutorialsFinished == 1 && SearchPanelsControl.GetInstance().GetIfPanelsEnabled())
        {
            AdvanceTutorial();
        }
        if (tutorialsFinished == 2 && !UniverseSystem.GetInstance().GetCurrentYear().Equals(UniverseSystem.LOBBY_YEAR_STRING))
        {
            AdvanceTutorial();
        }
        if (tutorialsFinished == 3 && CategoryManager.GetInstance().GetNumSelected() > 0)
        {
            AdvanceTutorial();
        }
	}

    public void SkipTutorials()
    {
        Debug.Log("Skipping Tutorials");
        foreach (GameObject tutorial in tutorialPanels) tutorial.SetActive(false);
        tutorialsFinished = tutorialPanels.Length;
        SearchPanelsControl.GetInstance().displayPanels();
    }

    public static TutorialController GetInstance()
    {
        return instance;
    }

    public void AdvanceTutorial()
    {
        if(tutorialsFinished < tutorialPanels.Length)
        {
            tutorialPanels[tutorialsFinished].SetActive(false);
            tutorialsFinished++;
            if (tutorialsFinished < tutorialPanels.Length)
            {
                tutorialPanels[tutorialsFinished].SetActive(true);
                tutorialPanels[tutorialsFinished].GetComponent<TutorialMove>().StartFollowing();
            }
        }
    }
}
