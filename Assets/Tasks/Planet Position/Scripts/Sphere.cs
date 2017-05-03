using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// start working on level 2 of planets
// scale everything by 18 bc the actual project uses scale 18 for planets and this project uses scale = 1 for planets rn
public class Sphere : MonoBehaviour {

    private float level1_Y = 7f;
    public GameObject prefab;
    private int num_of_planets = 8;
    public Material Planet1;
	public Material Planet2;
	public Material Planet3;
    public static int SPHERE_COUNT = 0;
    private GameObject planet;
	// Use this for initialization
	void Start ()
    { 
        setupSphere((float) 65/3, level1_Y, 6, Planet1);
		setupSphere((float) 65/3, 11f, 5, Planet2);
		setupSphere ((float)65/3, 14f, 6, Planet3);
	}

    public Vector3 getCartesianFor(float radius, float inclination, float azimuth)
    {
        return new Vector3(radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth), radius * Mathf.Cos(inclination), radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth));
    }

    public void setupSphere(float inputRadius, float inputY, int num, Material material)
    {
        GameObject bigSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
        bigSphere.GetComponent<SphereCollider>().radius = inputRadius;
        bigSphere.transform.position = new Vector3(0, 0, 0);
        Renderer rend = bigSphere.GetComponent<Renderer>();
        rend.enabled = false;

        float radius = bigSphere.GetComponent<SphereCollider>().radius * transform.localScale.x;

        float theta = (Mathf.PI / 2) - Mathf.Asin(inputY / radius);

        for (int i = 0; i < num; i++)
        {
            float sectorAngle = (Mathf.PI * 2) / num;

            Debug.Log(radius);
            Debug.Log(theta);
            Vector3 vect = getCartesianFor(radius, theta, i * sectorAngle);
            planet = Instantiate(prefab, vect, Quaternion.identity);
            planet.transform.localScale = new Vector3(2, 2, 2);
			planet.GetComponent<MeshRenderer> ().material = material;
        }
        SPHERE_COUNT++;
    }

}
