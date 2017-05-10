using UnityEngine;
using System.Collections;
using VRTK;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
 * OBSOLETE
 * TO BE REMOVED FROM FINAL PRODUCT
 */
public class Lever : VRTK_InteractableObject {

	bool leverOn = false;
	private Animator anima;

    private bool displayInfo;

    public Text titleOfScene;


    public override void StartUsing(GameObject usingObject)
    {
        base.StartUsing(usingObject);
        displayInfo = true;
        Debug.Log("StartUsing");
    }

    public override void StopUsing(GameObject usingObject)
    {
        base.StopUsing(usingObject);
        displayInfo = false;
        Debug.Log("StopUsing");
    }


    IEnumerator WaitLever() {
        for (float i = 0; i < 3; i += Time.deltaTime)
        {
            yield return null;
        }
        yield return new WaitForSecondsRealtime(2);
    }

    void OnTriggerStay(Collider other) {
        if (other.name == "Body") {
            Debug.Log("is player");
            if (displayInfo)
            {
                Debug.Log("left clicked");
                anima.SetBool("switch", true);
                anima.SetBool("switch 2", false);

                leverOn = true;
                StartCoroutine(WaitLever());
                SceneManager.LoadScene(titleOfScene.text);
            }
            else
            {
                Debug.Log("right clicked");
                anima.SetBool("switch", false);
                anima.SetBool("switch 2", true);
                leverOn = false;
            }
        }
		
	}

	// Use this for initialization
	void Start () { 
	
			anima = GetComponent<Animator>();
			

		
	}

    protected override void Update()
    {
        base.Update();
    }
}
