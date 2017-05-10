using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// start working on level 2 of planets
// scale everything by 18 bc the actual project uses scale 18 for planets and this project uses scale = 1 for planets rn
public class Sphere : MonoBehaviour {

    public Material Planet1;
	public Material Planet2;
	public Material Planet3;
	public int NUM_OF_PLANETS;
	private int tracker = 0;
	private float inputRadius = (float) 70 / 3;
	// Use this for initialization
	void Start ()
	{ 
		setupSphere (7f, 6, Planet1);
		setupSphere (13f, 5, Planet2);
		//setupSphere (18f, 6, Planet3);
	}

    public Vector3 getCartesianFor(float radius, float inclination, float azimuth)
    {
        return new Vector3(radius * Mathf.Sin(inclination) * Mathf.Sin(azimuth), radius * Mathf.Cos(inclination), radius * Mathf.Sin(inclination) * Mathf.Cos(azimuth));
    }

	public void setupSphere(float inputY, int num, Material material)
    {
        GameObject bigSphere = GameObject.CreatePrimitive (PrimitiveType.Sphere);
        bigSphere.GetComponent<SphereCollider>().radius = inputRadius;
        bigSphere.transform.position = new Vector3(0, 0, 0);
        Renderer rend = bigSphere.GetComponent<Renderer>();
        rend.enabled = false;
		int old_tracker = tracker;

        float radius = bigSphere.GetComponent<SphereCollider>().radius * transform.localScale.x;

        float theta = (Mathf.PI / 2) - Mathf.Asin(inputY / radius);

        for (int i = tracker; i < num + old_tracker; i++)
        {
            float sectorAngle = (Mathf.PI * 2) / num;

            Debug.Log(radius);
            Debug.Log(theta);
            Vector3 vect = getCartesianFor(radius, theta, i * sectorAngle);
			//list[i].transform.position = vect;
			//list[i].transform.localScale = new Vector3(2, 2, 2);
			//list[i].GetComponent<MeshRenderer> ().material = material;
			tracker++;
        }
        tracker++;
    }

}
