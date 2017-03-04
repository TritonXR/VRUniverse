using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using VRTK;

public class Controller : VRTK_InteractableObject
{

    //planet select
    public Text Title, Creator, Description, Year, Tag;
    public Image imageDes;
    private PlanetData planet_script;
    public GameObject UsingObject;

    //pointer preview
    public Text myText;
    //public float fadeTime;
    //public bool displayInfo;
    public Image panel;
    //public Color panelColor;
    //public Color textColor;

    public SteamVR_TrackedController rightController;

    // Use this for initialization
    protected void Start()
    {
        //Debug.Log("hello");
        panel.enabled = false;
        myText.enabled = false;

        planet_script = gameObject.GetComponent<PlanetData>();
        myText.text = planet_script.title;

        panel.transform.LookAt(Camera.main.transform);
    }

    
    private void HandleTriggerClicked(object sender, ClickedEventArgs e) {
        //var Cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //Cube.transform.position = transform.position;

        //Debug.Log("clicked");

        planet_script = gameObject.GetComponent<PlanetData>();

        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;
        Tag.text = planet_script.des_tag;
        myText.text = planet_script.title;

        imageDes.sprite = planet_script.image;
    }
    

    public override void StartUsing(GameObject currentUsingObject)
    {

        //Debug.Log("using");
        base.StartUsing(currentUsingObject);

        rightController.TriggerClicked += HandleTriggerClicked;

        /*
        UsingObject = currentUsingObject;
        planet_script = gameObject.GetComponent<PlanetData>();

        Title.text = planet_script.title;
        Creator.text = planet_script.creator;
        Description.text = planet_script.description;
        Year.text = planet_script.year;
        Tag.text = planet_script.des_tag;
        myText.text = planet_script.title;

        imageDes.sprite = planet_script.image;
        */


        //pointer preview
        //displayInfo = true;
        panel.enabled = true;
        myText.enabled = true;

        

    }

    /*
    void FadeText()

    {


        if (displayInfo)
        {
            planet_script = gameObject.GetComponent<PlanetData>();
            myText.color = Color.Lerp(myText.color, textColor, fadeTime * Time.deltaTime);
            panel.color = Color.Lerp(myText.color, panelColor, fadeTime * Time.deltaTime);

        }

        else
        {

            myText.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);
            panel.color = Color.Lerp(myText.color, Color.clear, fadeTime * Time.deltaTime);

        }




    }
    */

    public override void StopUsing(GameObject previousUsingObject)
    {
        //Debug.Log("stop");
        base.StopUsing(previousUsingObject);

        //pointer preview
        //displayInfo = false;

        StartUsing(UsingObject);

        panel.enabled = false;
        myText.enabled = false;

    }



    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        //FadeText();
    }
}