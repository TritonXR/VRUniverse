using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hyperspeed : MonoBehaviour {

    public float duration = 5.0F;
    public Color color0 = Color.red;
    public Color color1 = Color.blue;

    // Holds the particle system for hyperspeed
    private ParticleSystem hyperspeed;

    // The sound that plays when user hyperspeed activates to a year
    private AudioSource hyperspeedSound;

    // Directional Light in the scene will be controlled during hyperspeed
    private Light lt;

    // Use this for initialization
    void Start () {

        // Get the audiosource to access hyperspeed sound
        hyperspeedSound = gameObject.GetComponent<AudioSource>();

        // Get the light from the directional light child
        lt = GetComponentInChildren<Light>();

        // Get the hyperspeed particle system in this gameobject
        hyperspeed = gameObject.GetComponent<ParticleSystem>();

        // Pause the particle system because we don't want it to play automatically
        hyperspeed.Pause();

    }

    /*
     * Calls the StartCoroutine for Travel animation
     */
    public void StartHyperspeed()
    {
        StartCoroutine(Travel());
    }

    /*
     * Handles the playing and stopping of animation hyperspeed
     */
    IEnumerator Travel()
    {

        hyperspeed.Play();
        Debug.Log("play warp");
        if (lt != null)
        {
            hyperspeedSound.Play();
            for (float i = 0; i < 2; i += Time.deltaTime)
            {
                lt.intensity = Mathf.Lerp(1f, 0.5f, i / 2.0f);
                yield return null;
            }
        }
        yield return new WaitForSecondsRealtime(1);
        if (lt == null)
        {
            //cameraRig.transform.position = gameObject.transform.position;
        }
        hyperspeedSound.Stop();
        //SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
        yield return new WaitForSecondsRealtime(1);
        lightObject = GameObject.Find("Directional Light");
        if (lightObject != null)
        {
            //hyperspeedSound.Play();
            lt = lightObject.GetComponent<Light>();
            for (float i = 0; i < 2; i += Time.deltaTime)
            {
                lt.intensity = Mathf.Lerp(0.5f, 1f, i / 2.0f);
                yield return null;
            }
        }
        yield return new WaitForSecondsRealtime(1);
        //hyperspeedSound.Stop();
        hyperspeed.Stop();
        Debug.Log("stop warp");
        //yield return null; 

    }

}

    