using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class OverheadLightEffects : MonoBehaviour {
    public Light2D overhead;
    float baseIntensity = 0.9f;

    [SerializeField]
    public soundController soundController;

    void Start() {
        // the light controller should be active immediately
        StartCoroutine(LightController());
    }

    // function to pick values for light fading
    void FadeLights() {
        // picks a random amount of time between 1/10 of a second and 1/2 a second
        float randTime = Random.Range(0.1f, 0.5f);
        // picks a random light intensity between .4 and .8
        float randIntensity = Random.Range(0.4f, 0.8f);

        // uses the selected amount of time and light intensity to fade the lights
        StartCoroutine(FadeIntensity(randIntensity, randTime));
    }

    // funciton to flicker the overhead light
    IEnumerator FlickerLights() {
        // play sound effect of flickering light
        soundController.PlaySound(soundController.Sound.lightBuzz);
        // sets the light to an intensity of .3
        overhead.intensity = 0.3f;
        yield return new WaitForSeconds(0.1f);
        // after a 1/10 of a second, set it back to base value
        overhead.intensity = baseIntensity;
        yield return new WaitForSeconds(0.1f);
        // after a 1/10 of a second, set it to .5
        overhead.intensity = 0.5f;
        yield return new WaitForSeconds(0.1f);
        // after a 1/10 of a second, set it back to base value
        overhead.intensity = baseIntensity;
    }

    // function to fade the light intensity down and back up
    IEnumerator FadeIntensity(float randIntensity, float randTime) {
        // over the course of randTime seconds,
        for (float f = 0; f <= randTime; f += Time.deltaTime) {
            // slowly fade lights from default intensity to the selected intensity
            overhead.intensity = Mathf.Lerp(baseIntensity, randIntensity, f / randTime);
            yield return null;
        }
        // once that is done, make sure the intensity of the lights is at the selected random value
        overhead.intensity = randIntensity;
        // twice as fast as the lights faded,
        for (float f = 0; f <= randTime / 2; f += Time.deltaTime) {
            // bring the lights back up to the default value
            overhead.intensity = Mathf.Lerp(randIntensity, baseIntensity, f / randTime / 2);
            yield return null;
        }
        // make sure the intensity of the lights is now at the base value
        overhead.intensity = baseIntensity;
    }

    // function to control the overhead lights of the scene and perform effects
    IEnumerator LightController() {
        // loops the entire game
        while(true) {
            // waits a random amount of time between 15 and 20 seconds
            yield return new WaitForSeconds(Random.Range(15.0f, 20.0f));
            // picks a random float between 0 and 1
            float choice = Random.Range(0f, 1f);
            // there is a 40% chance for the lights to fade
            if (choice <= 0.4f) {
                FadeLights();
            // and a 60% chance for the lights to flicker
            } else {
                StartCoroutine(FlickerLights());
            }
        }
    }
}
