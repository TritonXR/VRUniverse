using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YearSelectGo : MonoBehaviour {

    [SerializeField]
    private int YearValue
    {
        get
        {
            return YearValue;
        }

        set
        {
            YearValue = value;
            YearString = value.ToString();
            YearText.text = YearString;
        }
    }
    [SerializeField] private Text YearText;
    private string YearString;
    

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    

}
