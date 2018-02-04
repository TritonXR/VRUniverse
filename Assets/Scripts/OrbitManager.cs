using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour {

    private static OrbitManager ORBIT_MANAGER = null;

	private const float FULL_CIRCLE = 2.0f * Mathf.PI; //const set for convenience and slight performance

	public float OrbitRadiusScaling = 10.0f;           //amount the radius is scaled with each additional planet
	public float OrbitRadiusBase = 300.0f;             //base radius
	public float PlanetDist = 150.0f;                  //how far the planets are from the player's orbit path
	public float AngleCeiling = 60.0f;                 //maximum angle planets can be above the horizon (in degrees)
	public float AngleFloor = -10.0f;                  //minimum angle planets can be above the horizon (in degrees)
	public float LinearSpeed = 20.0f;                  //speed at which the player's ship travels
    
	private Planet[] planet_list;                  //list of planet game objects
	public Light StarLight;                            //point light used to light the planets
	public Camera MainCamera;                          //main scene camera

	private float angularSpeed;
	private float polarLocation;
	private float orbitRadius;

	// Use this for initialization
	void Start () {
        if(ORBIT_MANAGER == null)
        {
            ORBIT_MANAGER = this;
        }
        else
        {
            Destroy(this);
        }

        PopulateOrbit(new Planet[0]);
	}
	
	// Update is called once per frame
	void Update () {
        //move the player a bit, make sure you don't overflow or run into other issues
		polarLocation += angularSpeed * Time.deltaTime;
		while (polarLocation > FULL_CIRCLE) polarLocation -= FULL_CIRCLE;

        //calculate and update player's position and rotation
		Vector3 position = new Vector3(orbitRadius * Mathf.Cos(polarLocation), transform.position.y, orbitRadius * Mathf.Sin(polarLocation));
		transform.position = position;
		transform.eulerAngles = new Vector3(0, 180.0f - (polarLocation * Mathf.Rad2Deg), 0);
	}

    public void PopulateOrbit(Planet[] planets)
    {
        planet_list = planets;
        //calculates various parameters for the orbit
        float theta = FULL_CIRCLE / planet_list.Length;
        orbitRadius = OrbitRadiusScaling * planet_list.Length + OrbitRadiusBase;
        angularSpeed = LinearSpeed / orbitRadius;

        StarLight.range = 1.2f * (orbitRadius + PlanetDist);          //adjust range of point light so it actually illuminates all the planets
        MainCamera.farClipPlane = 2.1f * (orbitRadius + PlanetDist);  //adjust the camera far plane so all planets are rendered

        for (int count = 0; count < planet_list.Length; count++)
        {
            float epsilon = Random.Range(AngleFloor, AngleCeiling); //how far above the horizon this planet will be

            //distance from star this planet will be (note that even counts are farther from star than player, odd counts are closer)
            float radius = orbitRadius + (1.0f - 2.0f * (count % 2)) * PlanetDist * Mathf.Cos(epsilon * Mathf.Deg2Rad);

            //distance above the player's orbit this planet will be
            float elevation = PlanetDist * Mathf.Sin(epsilon * Mathf.Deg2Rad);

            //calculate planet's actual position
            Vector3 position = new Vector3(radius * Mathf.Cos(theta * count), elevation, radius * Mathf.Sin(theta * count));

            planet_list[count].transform.position = position;

        }

        //player starts at angle 0
        polarLocation = 0.0f;
    }

    public static OrbitManager GetOrbitManager()
    {
        return ORBIT_MANAGER;
    }
}
