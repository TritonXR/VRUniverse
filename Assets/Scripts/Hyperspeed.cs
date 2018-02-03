using System.Collections;
using UnityEngine;

/*
 * Name: Hyperspeed.cs
 * Description: Contains methods for the blue particle light speed travel effect that happens when traveling between years
 * Utilized on: HyperspeedController particle system gameobject
 */

public class Hyperspeed : MonoBehaviour {

    // Holds the particle system for hyperspeed
    private ParticleSystem hyperspeed;

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
        hyperspeed = gameObject.GetComponent<ParticleSystem>();

        // Pause the particle system because we don't want it to play automatically
        hyperspeed.Pause();

    }

    /*
     * Travel: Handles the playing and stopping of animation hyperspeed
     * Parameters: bool forward - true if travelling to the year (start to fast)
     *                           false if slowly stopping the particles (fast to slow)
     */
    public IEnumerator Travel(bool forward)
    {

        // Play the animation particle system
        hyperspeed.Play();

        // Check if the light is null
        if (lt != null)
        {
            // Increase intensity of light and travel to the year sequence
            if (forward)
            {
                // Play the sound effect
                hyperspeedSound.Play();

                // Change the lighting in 2 seconds
                for (float i = 0; i < 2; i += Time.deltaTime)
                {
                    // Lerp to the lighting to see the change in a span of 2 seconds
                    lt.intensity = Mathf.Lerp(1f, 0.5f, i / 2.0f);
                    yield return null;
                }

            }

            // Decrease intensity of light and stop traveling to the year sequence
            else
            {
                // Change the lighting in 2 seconds
                for (float i = 0; i < 2; i += Time.deltaTime)
                {
                    // Lerp to the lighting to see the change in a span of 2 seconds
                    lt.intensity = Mathf.Lerp(0.5f, 1f, i / 2.0f);
                    yield return null;
                }
            }
        }

        // Wait for 1 second then stop the sound
        yield return new WaitForSecondsRealtime(1);

        // Stop the sound
        hyperspeedSound.Stop();

        // Stop the particle animation
        hyperspeed.Stop();
    }

}

    