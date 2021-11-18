using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cueball : MonoBehaviour {
    [SerializeField]
    public soundController soundController;

    public Transform cue;
    [SerializeField]
    public bool isMoving;

    public float timeMoving = 0.0f;

    // on Input.GetKeyUp(KeyCode.Space) in CueMove.cs, a function in this script should be called to
    // addForce to the rigidbody of the cueball, in the direction of the CircleCast from Trajectory.cs

    private void Update() {
        // if the cueball's velocity is nonzero
        if (this.GetComponent<Rigidbody2D>().velocity.magnitude > 0) {
            // set it as moving
            isMoving = true;
        } else {
            // otherwise set is as not moving
            isMoving = false;
        }

        // if the cueball is moving this frame
        if (isMoving) {
            // keep track of how long its moving for
            timeMoving += Time.deltaTime;
        } else {
            // if its not moving reset the timer
            timeMoving = 0.0f;
        }
    }

    // function to hit the cueball
    public void hitCueBall(float force) {
        // if the cueball is being hit at max force (0.25f)
        if (force == 0.25f) {
            // play hard hit
            soundController.PlaySound(soundController.Sound.ballCueHard, transform.position);
        } else {
            // otherwise play normal hit
            soundController.PlaySound(soundController.Sound.ballCueLight, transform.position);
        }

        // add an impulse force to the cueball in the direction that the cue is pointing
        this.GetComponent<Rigidbody2D>().AddForce(-cue.transform.right * force, ForceMode2D.Impulse);
    }

}
