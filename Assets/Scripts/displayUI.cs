using UnityEngine;
using UnityEngine.UI;
using System.Collections;
 
public class displayUI : MonoBehaviour {
 
    public string myString;
    public Text myText;
    public float fadeTime;
    public bool displayInfo;
    public Image h;
    public Color i;
    public Color j;

    
 
    // Use this for initialization
    void Start () {
     
        //myText = GameObject.Find ("Text").GetComponent<Text> ();
        //h = GameObject.Find ("Panel").GetComponent<Image>();
        i = new Color(.2f, .3f, .8f, .5f);
        j = new Color(0, 0, 0);
      
        myText.color = Color.clear;

     
        
  
      
        //Screen.showCursor = false;
        //Screen.lockCursor = true;
    }
     
    // Update is called once per frame
    void Update () 
    {
 
        FadeText ();
 
        /*if (Input.GetKeyDown (KeyCode.Escape)) 
         
                {
                        Screen.lockCursor = false;
                         
                }
                */
 
     
    }
 
    void OnMouseOver()
    {
        displayInfo = true;
 
    }
 
 
 
    void OnMouseExit()
 
    {
        displayInfo = false;
 
    }
 
 
    void FadeText ()
 
    {
 
 
        if(displayInfo)
        {
            
            myText.text = myString;
            myText.color = Color.Lerp (myText.color, j, fadeTime * Time.deltaTime);
            h.color = i;
          
        }
     
        else
        {
         
            myText.color = Color.Lerp (myText.color, Color.clear, fadeTime * Time.deltaTime);
            h.color = Color.Lerp (myText.color, Color.clear, fadeTime * Time.deltaTime);
           
        }
         
 
 
 
        }
 
 
 
}