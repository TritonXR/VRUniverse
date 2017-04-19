using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

    public Renderer rend;
    public float radius;
    public float level1_Z = 0.2f;
    public float level1_Y;
    public float level1_X;
    public Material Planet1;
    public GameObject sphere;
	// Use this for initialization
	void Start () {
        //sets the sphere to invis
        rend = GetComponent<Renderer>();
        rend.enabled = false;

        radius = GetComponent<SphereCollider>().radius * transform.localScale.x;
        //calculating x and y with the z
        float theta = Mathf.Asin(radius / level1_Z);
        level1_X = level1_Z * (1 / Mathf.Tan(theta));

        Vector3 vect = new Vector3(level1_X, level1_Z, radius);

        sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        sphere.transform.localScale = new Vector3(1, 1, 1);
        sphere.transform.position = vect;
        sphere.GetComponent<MeshRenderer>().material = Planet1;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
