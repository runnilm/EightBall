using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flickerControl : MonoBehaviour {
    public AudioSource audioSource;
    public AudioClip neonOn;
    public AudioClip neonOff;

    public Animator anim;

    // the coroutine should run as soon as the start scene begins
    void Start() {
        StartCoroutine(FlickerSounds());
    }

    // function to play the sounds of neon lights flickering
    IEnumerator FlickerSounds() {
        // loops during the entire start scene
        while (true) {
            // set the current audio clip as the neon light turning on sound
            audioSource.clip = neonOn;
            // begin the animation of the neon light flickering
            anim.SetBool("shouldFlicker", true);
            // play the sound of the neon light turning on
            audioSource.Play();
            // wait until the sound of neon light turning on finishes
            yield return new WaitWhile(() => audioSource.isPlaying);
            // make sure the sound ends
            audioSource.Stop();
            // stop the neon light flickering animation
            anim.SetBool("shouldFlicker", false);
            // set the audio clip as the neon light turning off
            audioSource.clip = neonOff;
            // play the sound
            audioSource.Play();
            // wait until the sound ends
            yield return new WaitWhile(() => audioSource.isPlaying);
            // stop the sound
            audioSource.Stop();
        }
    }
}
