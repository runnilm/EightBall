using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class gameController : MonoBehaviour {
    public bool gameIsStarted = false;
    public GameObject cbObj;
    public GameObject vhs;
    public GameObject soundController;
    public GameObject balls;
    public GameObject confetti;
    public Camera mainCam;

    private void Update() {
        // if the cueball is movinvg
        if (cbObj.GetComponent<Cueball>().isMoving) {
            // if the player clicks right mouse button
            if (Input.GetMouseButtonDown(1)) {
                // enable the vhs post processing effect
                vhs.SetActive(true);
                // pause all audio
                AudioListener.pause = true;
                // speed up time by 3x
                Time.timeScale = 3f;
            }
        }

        // if the cueball is not moving
        if (!cbObj.GetComponent<Cueball>().isMoving) {
            // disbale the vhs post processing effect
            vhs.SetActive(false);
            // unpause the audio
            AudioListener.pause = false;
            // set the time back to normal speed
            Time.timeScale = 1.0f;
        }

        // RESTART GAME IF BALLS ARE ALL SCORED
        if (balls.transform.childCount <= 1) {
            SceneManager.LoadScene("8Ball");
        }
    }
}
