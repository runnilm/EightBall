using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour {
    Vector3 initialPos;

    [SerializeField]
    public soundController soundController;
    public GameObject gameController;

    private void Awake() {
        initialPos = transform.position;
    }

    //when a ball collides with something
    private void OnCollisionEnter2D(Collision2D collision) {
        // grab the magnitude of the collision
        float magnitude = collision.relativeVelocity.magnitude;

        // if the ball has collided with another ball
        if (collision.gameObject.layer == LayerMask.NameToLayer("balls")) {
            //Debug.Log(this.gameObject.name + " collided with " + collision.gameObject.name 
            //    + " at magnitude " + collision.relativeVelocity.magnitude);

            // if the magnitude of the collision is greater than 2.0f
            if (magnitude >= 2.0f) {
                // play hard hit
                soundController.PlaySound(soundController.Sound.ballBallHard, transform.position);
            // if magnitude of collision is > 1.0f but < 2.0f
            } else if (magnitude >= 1.0f) {;
                // play medium hit
                soundController.PlaySound(soundController.Sound.ballBallMedium, transform.position);
            // if magnitude of collision is > 0.1f but < 1.0f
            } else if (magnitude >= 0.1f) {
                // play light hit
                soundController.PlaySound(soundController.Sound.ballBallLight, transform.position);
            }
        }

        // if the ball has collided with the edge of the table
        if (collision.gameObject.layer == LayerMask.NameToLayer("edgecollider")) {
            //Debug.Log(this.gameObject.name + " collided with " + collision.gameObject.name
            //    + " at magnitude " + collision.relativeVelocity.magnitude);

            // if the magnitude of the collision is greater than 1.75f
            if (magnitude >= 1.75f) {
                // play hard hit
                soundController.PlaySound(soundController.Sound.ballEdgeHard, transform.position);
            }
            // if magnitude of collision is > 1.0f but < 1.75f
            else if (magnitude >= 1.0f) {
                // play medium hit
                soundController.PlaySound(soundController.Sound.ballEdgeMedium, transform.position);
            // if magnitude of collision is > 0.1f but < 1.0f
            } else if (magnitude >= 0.1f) {
                // play light hit
                soundController.PlaySound(soundController.Sound.ballEdgeLight, transform.position);
            }

        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        // if the ball (non-cueball) collides with a goal
        if (collision.GetComponent<Collider2D>().tag == "goal" && this.gameObject.tag != "cueball") {
            // destroy the ball
            Destroy(this.gameObject);
        }

        // if the cueball collides with a goal
        if (this.gameObject.tag == "cueball") {
            // remove all velocity from the cueball
            this.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            this.gameObject.GetComponent<Rigidbody2D>().angularVelocity = 0;
            // move the cueball back to the start line
            this.gameObject.transform.position = initialPos;
            // the game is not longer "started", so the startline should show again
            gameController.GetComponent<gameController>().gameIsStarted = false;
        }
    }
}
