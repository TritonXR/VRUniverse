using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {
	public float x;
	public float y;
	public float z;
	
	void Update () {
        //transform.Rotate(new Vector3(x*30, y*60, z*90) * Time.deltaTime);
	Vector3 currentPos = transform.position; 
	transform.position = (currentPos + new Vector3(x*1, y*1, z*1) * Time.deltaTime); 	
	}
	
}
