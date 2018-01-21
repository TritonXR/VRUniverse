using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour {

	public float OrbitRadius = 500.0f;
	public float PlanetDist = 200.0f;
	public float AngleCeiling = 60.0f;
	public float AngleFloor = -10.0f;

	public int num_planets = 20;
	public GameObject prefab;

	public float angularSpeed = 6.0f; //degrees per second

	public float polarLocation;

	// Use this for initialization
	void Start () {

		float theta = (2 * Mathf.PI) / num_planets;

		for (int count = 0; count < num_planets; count++)
		{
			float epsilon = Random.Range(AngleFloor, AngleCeiling);

			float radius = OrbitRadius + (1.0f - 2.0f * (count % 2)) * PlanetDist * Mathf.Cos(epsilon * Mathf.Deg2Rad);
			float elevation = PlanetDist * Mathf.Sin(epsilon * Mathf.Deg2Rad);

			Vector3 position = new Vector3(radius * Mathf.Cos(theta * count), elevation, radius * Mathf.Sin(theta * count));

			Instantiate(prefab, position, Quaternion.identity);
			
		}

		polarLocation = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		polarLocation += angularSpeed * Time.deltaTime;

		Vector3 position = new Vector3(OrbitRadius * Mathf.Cos(polarLocation * Mathf.Deg2Rad), transform.position.y, OrbitRadius * Mathf.Sin(polarLocation * Mathf.Deg2Rad));
		transform.position = position;
		transform.eulerAngles = new Vector3(0, 180 - polarLocation, 0);
	}
}
