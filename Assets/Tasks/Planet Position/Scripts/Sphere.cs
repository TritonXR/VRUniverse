using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// change CreatePrimitives to Prefabs
// start working on level 2 of planets
// scale everything by 18 bc the actual project uses scale 18 for planets and this project uses scale = 1 for planets rn
public class Sphere : MonoBehaviour {

    public Renderer rend;
    private float radius;
    private float level1_Z = 0;
    private float level1_Y = 3f;
    private float level1_X;
    private int num_of_planets = 1;
    public Material Planet1;
    public GameObject planet;
	// Use this for initialization
	void Start () {
        //sets the sphere to invis
        rend = GetComponent<Renderer>();
        rend.enabled = false;
        //GetComponent<SphereCollider>().radius = 1;
        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;

        float theta = (Mathf.PI / 2) - Mathf.Asin(level1_Y / radius);
        level1_X = radius * Mathf.Cos(theta);
        for (int i = 0; i < num_of_planets; i++)
        {
            float sectorAngle = (Mathf.PI * 2) / num_of_planets;
           
            Vector3 vect = getCartesianFor(radius, theta, i * sectorAngle);
        
            planet = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            planet.transform.localScale = new Vector3(1, 1, 1);
            planet.transform.position = vect;
            planet.GetComponent<MeshRenderer>().material = Planet1;
        }
	}

    public Vector3 getCartesianFor(float radius, float inclination, float azimuth)
    {
        return new Vector3(radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth), radius * Mathf.Cos(inclination), radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth));
    }
}
