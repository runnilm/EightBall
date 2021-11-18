using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CueMove : MonoBehaviour {
    [SerializeField]
    public GameObject gameController;
    public GameObject cbObj;
    public Transform cbTransform;
    public Cueball cbScript;
    public Vector3 diff;

    private float angle;
    private Vector3 initialPos;
    public bool isFiring = false;
    public bool isTransitioning = false;
    public bool canBeShot = true;

    private float moveSpeed = 5f;
    private float distance;

    void Update() {
        // controlling the cue:
        // - moving mouse around adjusts angle of cue around cue ball
        // - holding space locks angle of cue and allows for power adjustment
        //   - the cue should rotate around the cue ball
        //   - moving the cue away from the cue ball should control power
        //     - there should be a max range from the cue (and therefore max power)
        // - releasing space / 'shooting' should return the cue to original pos smoothly/quickly
        // - pressing space while holding left click should 'shoot'

        // get current mouse position
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        // get the vector between the mouse and the cueball transform
        diff = mousePos - cbTransform.position;
        // find the angle that the cue should be at in radians and convert it to degrees
        angle = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        // if not pressing space and the cue isnt moving and the cueball isnt moving
        if (!Input.GetKey(KeyCode.Space) && !isTransitioning && !cbScript.isMoving) {
            // reposition the cue
            StartCoroutine(Reposition());
        }

        // if space is let go and the cueball isnt moving and can be shot
        if (Input.GetKeyUp(KeyCode.Space) && !cbScript.isMoving && canBeShot) {
            // set the cue as moving
            isTransitioning = true;
                
            // "reposition" cue to give the effect of the cue hitting the cueball
            if (!cbScript.isMoving) {
                StartCoroutine(Reposition());
            }

            // shoot the cueball
            StartCoroutine(BeginShot());

            // once the cueball has been shot, no longer firing
            isFiring = false;
        }

        // if holding space and the cueball isnt moving and can be shot
        if (Input.GetKey(KeyCode.Space) && !cbScript.isMoving && canBeShot) {
            // if not currently firing
            if (!isFiring) {
                // grab the position the cue starts at
                initialPos = transform.position;
            }
            // now we are firing
            isFiring = true;
            // grab the current position of the cue
            Vector3 currentPos = transform.position;

            // if the distance between the starting position and the current position
            // of the cue is less than 0.25f
            if (Vector3.Distance(initialPos, currentPos) <= 0.25f) {
                // move the cue pivot to its right (along red axis) at a predefined speed (this is a single step)
                transform.parent.position += transform.right * Time.deltaTime * 0.5f;
            }

            // get the distance between the current position of the cue and the starting position
            distance = Vector3.Distance(initialPos, currentPos);

            // if this distance is greater than 0.25f, clamp it to 0.25f (max power shot)
            if (distance > 0.25f) {
                distance = 0.25f;
            }
        }

        // function to reposition the cue
        IEnumerator Reposition() {
            // grab the current position and rotation of the cue pivot
            Vector3 currentPos = transform.parent.position;
            Quaternion currentRot = transform.parent.rotation;

            // value to use for Lerp function
            var t = 0f;
            // basically while the cue has not yet reached the position of the cueball (1 = at position)
            while (t < 1) {
                // increase value of t by defined moveSpeed
                t += Time.deltaTime * moveSpeed;

                // make sure t does not go over 1
                if (t > 1 ) {
                    t = 1;
                }
                
                // step the position and rotation of the cue pivot closer to the cueball
                transform.parent.position = Vector3.Lerp(currentPos, cbTransform.position, t);
                transform.parent.rotation = Quaternion.Lerp(currentRot, Quaternion.Euler(0.0f, 0.0f, angle), t);
                // repeats until cueball is reached
                yield return null;
            }

            // once back in position, the cue can be shot again and is no longer transitioning to the cueball
            canBeShot = true;
            isTransitioning = false;
        }

        // function to shoot the cueball
        IEnumerator BeginShot() {
            // since we are now shooting, it can no longer be shot
            canBeShot = false;

            // waits until the cue pivot has reached the cueball
            while (transform.parent.position != cbTransform.position) {
                yield return null;
            }

            // hit the cueball as a function of the distance between cueball and cue (how far it was pulled back)
            cbObj.GetComponent<Cueball>().hitCueBall(distance);
            // the game has started if the cue is being hit
            gameController.GetComponent<gameController>().gameIsStarted = true;
        }
    }
}
