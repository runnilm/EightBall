using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class startLineRender : MonoBehaviour {
    [SerializeField]
    public GameObject gameController;

    private LineRenderer startLine;

    void Awake() {
        // generates the start line at the correct position on the table
        startLine = this.GetComponent<LineRenderer>();
        EnableStartLine();
        Vector3[] startLinePos = new Vector3[2] { Camera.main.ViewportToWorldPoint(new Vector3(.67f, .78f, 1)),
                                                  Camera.main.ViewportToWorldPoint(new Vector3(.67f, .22f, 1)) };
        float width = startLine.startWidth;
        startLine.material.mainTextureScale = new Vector2(1.0f / width, 1.1f);
        startLine.SetPositions(startLinePos);
    }

    private void Update() {
        // if the game has started (the cue has been hit off of the start line)
        if (gameController.GetComponent<gameController>().gameIsStarted) {
            // disable the start line
            DisableStartLine();
        } else {
            // otherwise show the start line
            EnableStartLine();
        }
    }

    // function to enable startline
    void EnableStartLine() {
        startLine.enabled = true;
    }

    // function to disable startline
    void DisableStartLine() {
        startLine.enabled = false;
    }
}
