using System.Collections;
using UnityEngine;

/*
 * Name: Hyperspeed.cs
 * Description: Contains methods for the blue particle light speed travel effect that happens when traveling between years
 * Utilized on: HyperspeedController particle system gameobject
 */
   

public class Hyperspeed : MonoBehaviour {

    [SerializeField] Transform spaceShip;
    [SerializeField] Light pointLight;
    [SerializeField] Light dirLight;
    [SerializeField] AudioClip departureSound;
    [SerializeField] AudioClip arrivalSound;

    // Holds the particle system for hyperspeed
    private ParticleSystem hyperspeedStart;
    private ParticleSystem hyperspeedEnd;

    // The sound that plays when user hyperspeed activates to a year
    private AudioSource hyperspeedSound;

    // Directional Light in the scene will be controlled during hyperspeed
    private Light lt;

    /*
     * Start: Initialize sound, light, particles. Start off the system as pause.
     * Parameters: None
     */
    void Start () {

        // Get the audiosource to access hyperspeed sound
        hyperspeedSound = gameObject.GetComponent<AudioSource>();

        // Get the light from the directional light child
        lt = GetComponentInChildren<Light>();

        // Get the hyperspeed particle system in this gameobject
        hyperspeedStart = gameObject.GetComponent<ParticleSystem>();

        // Pause the particle system because we don't want it to play automatically
        hyperspeedStart.Pause();

        //Vector3 rot = spaceShip.rotation.eulerAngles;
        //rot = new Vector3(rot.x, rot.y + 180, rot.z);
        //gameObject.transform.rotation = Quaternion.Euler(rot);
       

    }

    void Update ()
    {
        //gameObject.transform.position = new Vector3(spaceShip.position.x + 30.0f, spaceShip.position.y, spaceShip.position.z);

        //Vector3 rot = spaceShip.rotation.eulerAngles;
        //gameObject.transform.rotation.rot
        //rot = new Vector3(rot.x, gameObject.transform.rotation.y, rot.z);         
        //gameObject.transform.rotation = Quaternion.Euler(rot);

        

    }

    /*
     * Travel: Handles the playing and stopping of animation hyperspeed
     * Parameters: bool forward - true if travelling to the year (start to fast)
     *                           false if slowly stopping the particles (fast to slow)
     */
    public IEnumerator Travel(bool forward)
    {

        // Play the animation particle system
        if(forward) hyperspeedStart.Play();
        //if (true) hyperspeed.startLifetime = 15;
        //else hyperspeed.main.
        
        // Check if the light is null
        if (dirLight != null)
        {
            if (forward) hyperspeedSound.clip = departureSound;
            else hyperspeedSound.clip = arrivalSound;

            // Increase intensity of light and travel to the year sequence
            if (forward)
            {
                Debug.Log("Going forward hyperspeed");               

                // Play the sound effect
                hyperspeedSound.Play();
                pointLight.enabled = false;
                //dirLight.enabled = false;

                // Change the lighting in 2 seconds
                for (float i = 0; i < 3; i += Time.deltaTime)
                {
                    // Lerp to the lighting to see the change in a span of 2 seconds
                    dirLight.intensity = Mathf.Lerp(1f, 0.3f, i / 2.0f);
                    yield return null;
                }

            }

            // Decrease intensity of light and stop traveling to the year sequence
            else
            {
                Debug.Log("Arriving in hyperspeed");
                hyperspeedSound.Play();

                // Change the lighting in 2 seconds
                for (float i = 0; i < 1; i += Time.deltaTime)
                {
                    // Lerp to the lighting to see the change in a span of 2 seconds
                    dirLight.intensity = Mathf.Lerp(0.3f, 1f, i / 2.0f);
                    yield return null;
                }
                //dirLight.enabled = true;
                pointLight.enabled = true;
            }
        }

        // Stop the particle animation
        if(!forward) hyperspeedStart.Stop();

        // Wait for 1 second then stop the sound
        if (forward) yield return new WaitForSecondsRealtime(1);
        else yield return new WaitForSecondsRealtime(3);
       

        // Stop the sound
        hyperspeedSound.Stop();

        


    }

}

    