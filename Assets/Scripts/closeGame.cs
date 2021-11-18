using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closeGame : MonoBehaviour
{
    // simple script to close the game when escape key is pressed
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
