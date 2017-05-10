using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * OBSOLETE
 * TO BE REMOVED FROM FINAL PRODUCT
 */

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

}
