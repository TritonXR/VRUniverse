using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour {

    public static AudioSource music = null;
    
    private void Awake()
    {
        if (music == null) {
            music = gameObject.GetComponent<AudioSource>();
            music.Play();
        } else if (music != gameObject.GetComponent<AudioSource>()) {
            Destroy(gameObject);
        }

    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
