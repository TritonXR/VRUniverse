using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitManager : MonoBehaviour {

	private const float FULL_CIRCLE = 2.0f * Mathf.PI;

	public float OrbitRadiusScaling = 10.0f;
	public float OrbitRadiusBase = 300.0f;
	public float PlanetDist = 150.0f;
	public float AngleCeiling = 60.0f;
	public float AngleFloor = -10.0f;
	public float LinearSpeed = 20.0f;

	public int num_planets = 20;
	public GameObject prefab;
	public Light StarLight;
	public Camera MainCamera;

	private float angularSpeed;
	private float polarLocation;
	private float orbitRadius;

	// Use this for initialization
	void Start () {

		float theta = FULL_CIRCLE / num_planets;
		orbitRadius = OrbitRadiusScaling * num_planets + OrbitRadiusBase;
		angularSpeed = LinearSpeed / orbitRadius;
		StarLight.range = 1.2f * (orbitRadius + PlanetDist);
		MainCamera.farClipPlane = 2.1f * (orbitRadius + PlanetDist);

		for (int count = 0; count < num_planets; count++)
		{
			float epsilon = Random.Range(AngleFloor, AngleCeiling);

			float radius = orbitRadius + (1.0f - 2.0f * (count % 2)) * PlanetDist * Mathf.Cos(epsilon * Mathf.Deg2Rad);
			float elevation = PlanetDist * Mathf.Sin(epsilon * Mathf.Deg2Rad);

			Vector3 position = new Vector3(radius * Mathf.Cos(theta * count), elevation, radius * Mathf.Sin(theta * count));

			Instantiate(prefab, position, Quaternion.identity);
			
		}

		polarLocation = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		polarLocation += angularSpeed * Time.deltaTime;
		while (polarLocation > FULL_CIRCLE) polarLocation -= FULL_CIRCLE;

		Vector3 position = new Vector3(orbitRadius * Mathf.Cos(polarLocation), transform.position.y, orbitRadius * Mathf.Sin(polarLocation));
		transform.position = position;
		transform.eulerAngles = new Vector3(0, 180.0f - (polarLocation * Mathf.Rad2Deg), 0);
	}
}
