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
    [SerializeField]
    private GameObject tutorialSpawnPoint; // spawn point

    [SerializeField] private float tutorialHoldTime = 1.0f;
    [SerializeField] private float tutorialMoveTime = 1.0f;

    int tutorialsFinished = 0;

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
        welcomeTutorial_1.SetActive(false);
        yearTutorial_2.SetActive(false);
        categoryTutorial_3.SetActive(false);
        searchTutorial_4.SetActive(false);

        switch(tutorialsFinished)
        {
            case 0:
                StartCoroutine(SpawnAndMoveTutorial(welcomeTutorial_1));
                break;
            case 1:
                StartCoroutine(SpawnAndMoveTutorial(yearTutorial_2));
                break;
            case 2:
                StartCoroutine(SpawnAndMoveTutorial(categoryTutorial_3));
                break;
            case 3:
                StartCoroutine(SpawnAndMoveTutorial(searchTutorial_4));
                break;
            default:
                break;
        }

    }
	
	// Update is called once per frame
	void Update () {
		if(tutorialsFinished == 0 && Mathf.Abs(OrbitManager.GetOrbitManager().LinearSpeed) < FP_TOLERANCE)
        {
            Debug.Log("Finished first tutorial");
            tutorialsFinished++;
            welcomeTutorial_1.SetActive(false);
            StartCoroutine(SpawnAndMoveTutorial(yearTutorial_2));
        }

        if (tutorialsFinished == 1 && !UniverseSystem.GetInstance().GetCurrentYear().Equals(UniverseSystem.LOBBY_YEAR_STRING))
        {
            Debug.Log("Finished second tutorial");
            tutorialsFinished++;

            yearTutorial_2.SetActive(false);
            StartCoroutine(SpawnAndMoveTutorial(categoryTutorial_3));
        }

        if(tutorialsFinished == 2 && CategoryManager.GetInstance().GetNumSelected() > 0)
        {
            Debug.Log("Finished third tutorial");
            tutorialsFinished++;

            categoryTutorial_3.SetActive(false);
            StartCoroutine(SpawnAndMoveTutorial(searchTutorial_4));
        }

	}

    public void SkipTutorials()
    {
        Debug.Log("Skipping Tutorials");
        tutorialsFinished = 4;
        welcomeTutorial_1.SetActive(false);
        yearTutorial_2.SetActive(false);
        categoryTutorial_3.SetActive(false);
        searchTutorial_4.SetActive(false);
    }

    public static TutorialController GetInstance()
    {
        return instance;
    }

    private IEnumerator SpawnAndMoveTutorial(GameObject tutorial)
    {
        Vector3 targetPosition = tutorial.transform.localPosition;
        Quaternion targetRotation = tutorial.transform.localRotation;
        Vector3 startPosition = tutorialSpawnPoint.transform.position;
        Quaternion startRotation = tutorialSpawnPoint.transform.rotation;
        
        if (tutorial.transform.parent != null)
        {
            startPosition = tutorial.transform.parent.InverseTransformPoint(startPosition);
            startRotation = Quaternion.Inverse(tutorial.transform.parent.rotation) * startRotation;
        }

        float currInterpolation = 0.0f;
        tutorial.transform.localPosition = startPosition;
        tutorial.transform.localRotation = startRotation;
        tutorial.SetActive(true);

        yield return new WaitForSeconds(tutorialHoldTime);

        while(currInterpolation < 1.0f)
        {
            yield return null;
            currInterpolation += Time.deltaTime / tutorialMoveTime;
            tutorial.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, currInterpolation);
            tutorial.transform.localRotation = Quaternion.Slerp(startRotation, targetRotation, currInterpolation);
        }
    }
}
