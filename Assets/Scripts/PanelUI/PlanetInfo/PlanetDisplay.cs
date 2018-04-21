using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlanetDisplay : MonoBehaviour {

    private static PlanetDisplay instance;

    [SerializeField] private Text Planet_Title, Planet_Creator, Planet_Description, Planet_Year, Planet_Tag;
    [SerializeField] private Image Planet_Image;
    [SerializeField] private Transform targetViewer; //the canvas will turn to face this transform
    [SerializeField] private Transform orbitAnchor; //the canvas will orbit around this transform
    [SerializeField] private float orbitRadius; //how far the canvas is from the orbit anchor
    [SerializeField] private Vector2 baseOffsetDirection; //this gets normalized, so you just need the direction roughly correct
    [SerializeField] private Vector2 additionalOffset; //this doesn't get normalized, it's in addition to the base offset
    [SerializeField] private TravelInteractable travelConfirmButton; 
    [SerializeField] private ExitInteractable exitButton;
    public Transform targetPlanet;
    private Canvas renderedCanvas;
	private BoxCollider[] buttonColliders;

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
        targetPlanet = null;
        //baseOffsetDirection.Normalize();
        renderedCanvas = GetComponent<Canvas>();
        renderedCanvas.enabled = false;
		buttonColliders = GetComponentsInChildren<BoxCollider> ();
		foreach (BoxCollider col in buttonColliders) {
			col.enabled = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(targetPlanet != null)
        {
            Vector3 targetPlanetOffset = targetPlanet.position - orbitAnchor.position;
            Vector3 rightOffsetDir = Vector3.Cross(targetPlanetOffset, Vector3.up).normalized;
            Vector3 upOffsetDir = Vector3.Cross(rightOffsetDir, targetPlanetOffset).normalized;
            //Debug.Log("Distance to target: " + targetPlanetOffset.magnitude);

            float planetRadius = Vector3.Dot(targetPlanet.localScale, Vector3.one) / 6.0f; //divide by 3 to get average scaling, divide by 2 to get from diameter to radius
            //Debug.Log("Planet radius: " + planetRadius);
            float radiusForOffset = planetRadius / targetPlanetOffset.magnitude * orbitRadius;
            //Debug.Log("radiusForOffset: " + radiusForOffset);
            float rightOffset = baseOffsetDirection.x * radiusForOffset + additionalOffset.x;
            //Debug.Log("rightOffset: " + rightOffset);
            float upOffset = baseOffsetDirection.y * radiusForOffset + additionalOffset.y;
            //Debug.Log("upOffset: " + upOffset);
            transform.position = targetPlanetOffset.normalized * orbitRadius + rightOffsetDir * rightOffset + upOffsetDir * upOffset + orbitAnchor.position;

            float directionAngle = Mathf.Atan2(transform.position.x - orbitAnchor.position.x, transform.position.z - orbitAnchor.position.z) * Mathf.Rad2Deg;
            float elevationAngle = Mathf.Atan2(targetPlanetOffset.y, Mathf.Sqrt(targetPlanetOffset.x * targetPlanetOffset.x + targetPlanetOffset.z * targetPlanetOffset.z)) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(-elevationAngle, directionAngle, 0.0f);
        }
    }

    public void SetViewTarget(Transform target)
    {
        targetPlanet = target;
    }

    public Transform GetViewTarget()
    {
        return targetPlanet;
    }

    public void UpdateInfo(string title, string creator, string desc, string year, string[] tags, Sprite image)
    {
        //Sets the text for the different data components on the menu
        Planet_Title.text = title;
        Planet_Creator.text = creator;
        Planet_Description.text = desc;
        Planet_Year.text = year;

        //Must be handled differently because tags are stored as an array and we must concatenate them
        string tagText = "";
        for (int i = 0; i < tags.Length; i++)
        {
            if (i == tags.Length - 1)
            {
                tagText = tagText + tags[i];
            }
            else
            {
                tagText = tagText + tags[i] + ", ";
            }

        }

        Planet_Tag.text = tagText;
        if(image != null)
        {
            Planet_Image.enabled = true;
            Planet_Image.sprite = image; //Uses the image component to set the sprite of what the picture should be
        }
        else
        {
            Planet_Image.enabled = false;
        }
        
    }

    public void SetVisible(bool visible)
    {
        if(renderedCanvas != null) renderedCanvas.enabled = visible;
        if (buttonColliders != null)
        {
            foreach (BoxCollider col in buttonColliders)
            {
                col.enabled = visible;
            }
        }
    }

    public ExitInteractable GetExitInteractable()
    {
        return exitButton;
    }

    public TravelInteractable GetTravelInteractable()
    {
        return travelConfirmButton;
    }

    public static PlanetDisplay GetInstance()
    {
        return instance;
    }
}
