using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class soundController : MonoBehaviour {
    // array that holds all of the sound audio clips, which include a sound enum and an audio clip
    public SoundAudioClip[] soundAudioClipArray;

    [System.Serializable]
    // attach a sound name from the enum and an audioclip to a soundaudioclip
    public class SoundAudioClip {
        public soundController.Sound sound;
        public AudioClip audioClip;
    }

    // enum to handle the names of various sound effects
    public enum Sound {
        ballBallLight,
        ballBallMedium,
        ballBallHard,

        ballEdgeLight,
        ballEdgeMedium,
        ballEdgeHard,

        ballCueLight,
        ballCueHard,

        confettiPop,

        lightBuzz
    }

    // function to play a sound using the associated sound name (from enum) and position (for some spatial sound)
    public void PlaySound(Sound sound, Vector3 position) {
        // create a game object for the current sound being played
        GameObject soundGameObject = new GameObject("Sound");
        // set the gameobject as a child of the soundController for organization
        soundGameObject.transform.SetParent(GameObject.Find("soundController").transform);
        // set the position of the gameobject at the position received by the function call
        soundGameObject.transform.position = position;

        // create a new audiosource component on the created gameobject
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        // enable spatial sound
        audioSource.spatialBlend = 1;
        // enable linear volume rolloff (sound gets slightly quieter as it moves away)
        audioSource.rolloffMode = AudioRolloffMode.Linear;
        // disable doppler effect
        audioSource.dopplerLevel = 0;
        // set the audio clip for the audiosource as the one given by the function call
        audioSource.clip = GetAudioClip(sound);

        // if the game is currently being fast forwarded
        if (Time.timeScale == 3f) {
            // destroy the create sound before its played
            Destroy(soundGameObject);
        } else {
            // if time is progressing normally, play the sound
            StartCoroutine(HandleSound(soundGameObject, audioSource));
        }
    }

    // function to play a sound without spatial sound
    public void PlaySound(Sound sound) {
        // create gameobject for the sound
        GameObject soundGameObject = new GameObject("Sound");
        // set as child of soundController
        soundGameObject.transform.SetParent(GameObject.Find("soundController").transform);

        // create audiosource component of sound gameobject
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        // play the sound effect once
        audioSource.PlayOneShot(GetAudioClip(sound));
    }

    // function to grab the audioclip associated with the sound enum
    private AudioClip GetAudioClip(Sound sound) {
        // iterate through each soundaudioclip in the soundaudiocliparray
        foreach (SoundAudioClip soundAudioClip in soundAudioClipArray) {
            // look for matching sound audio clip
            if (soundAudioClip.sound == sound) {
                // return the audioclip of that sound audio clip
                return soundAudioClip.audioClip;
            }
        }
        // this will never be reached if existing enum is used
        return null;
    }

    // function to "handle" the sound, meaning play and make sure the object is destroyed after playing
    IEnumerator HandleSound(GameObject soundGameObject, AudioSource audioSource) {
        //plays the audio
        audioSource.Play();
        // waits until the audio is finished playing
        yield return new WaitWhile(() => audioSource.isPlaying);
        // destroys the sound game object after the sound is finished playing
        Destroy(soundGameObject);
        yield return null;
    }

}
