using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSkip : MonoBehaviour, PointableObject
{

    [SerializeField] private Text buttonText;
    [SerializeField] private TutorialMove moveController;
    private Image buttonBorder;
    [SerializeField] private Color TextDefaultColor = Color.white;
    [SerializeField] private Color TextHighlightColor = Color.black;

    // Use this for initialization
    void Start()
    {
        buttonBorder = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PointerEnter()
    {
        buttonBorder.fillCenter = true;
        buttonText.color = TextHighlightColor;
    }

    public void PointerClick()
    {
        if (moveController.IsFollowing()) moveController.StopFollowing();
        TutorialController.GetInstance().SkipTutorials();
    }

    public void PointerExit()
    {
        buttonBorder.fillCenter = false;
        buttonText.color = TextDefaultColor;
    }
}
