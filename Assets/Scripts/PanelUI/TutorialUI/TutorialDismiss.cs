using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialDismiss : MonoBehaviour, PointableObject
{

    [SerializeField] private Text buttonText;
    [SerializeField] private TutorialMove moveController;
    private Image buttonBorder;
    [SerializeField] private Color TextDefaultColor = Color.white;
    [SerializeField] private Color TextHighlightColor = Color.black;

    private bool dismissed;

    // Use this for initialization
    void Start()
    {
        buttonBorder = GetComponent<Image>();
        dismissed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (dismissed && moveController.IsFollowing())
        {
            dismissed = false;
            buttonText.text = "Dismiss";
        }
    }

    public void PointerEnter()
    {
        buttonBorder.fillCenter = true;
        buttonText.color = TextHighlightColor;
    }

    public void PointerClick()
    {
        if(dismissed)
        {
            TutorialController.GetInstance().AdvanceTutorial();
        }
        else
        {
            dismissed = true;
            moveController.StopFollowing();
            buttonText.text = "Next Tutorial";
        }
    }

    public void PointerExit()
    {
        buttonBorder.fillCenter = false;
        buttonText.color = TextDefaultColor;
    }
}
