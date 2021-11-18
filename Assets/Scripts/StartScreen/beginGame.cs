using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class beginGame : MonoBehaviour {
    void Update() {
        // if any key is pressed
        if (Input.anyKeyDown) {
            // start the game
            SceneManager.LoadScene("8Ball");
        }
    }
}
