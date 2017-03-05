using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpEffectSave : MonoBehaviour {

    public static GameObject particleWarp = null;

    void Awake()
    {       
        if (particleWarp == null)
        {
            particleWarp = gameObject;
        }
        else if (particleWarp != gameObject)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
