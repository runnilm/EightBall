using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trajectory : MonoBehaviour {
    public Camera cam;
    public LineRenderer trajectory;
    public LineRenderer trajectoryShade;
    public Transform startPoint;
    public Transform startPointShade;
    public Transform cueBall;
    public Transform trajCircleTransform;
    public GameObject trajectoryCircle;
    public GameObject cue;

    public RaycastHit2D ballHit;

    public float moveSpeed = 1.5f;

    void Update() {
        // if you are not pressing space and the cue is not repositioning to the cueball and the cueball isn't moving
        if (!Input.GetKey(KeyCode.Space) && !cue.GetComponent<CueMove>().isTransitioning
            && !cueBall.GetComponent<Cueball>().isMoving) {
            // enable the trajectory line and update it
            EnableTrajectory();
            UpdateTrajectory();
        }
        // if the cue is repositioning or the cueball is moving *********
        if (cue.GetComponent<CueMove>().isTransitioning || cueBall.GetComponent<Cueball>().isMoving) {
            // disable the trajectory line
            DisableTrajectory();
        }
    }

    // updates the trajectory line
    void UpdateTrajectory() {
        // trajectory render
        // gets the vector between the mouse position and the cueball position
        Vector3 diff = cam.ScreenToWorldPoint(Input.mousePosition) - cueBall.position;
        // sends a circlecast out in the direction the cue is pointing, only stopping when it hits a ball (non-cueball)
        // or the edge of the table
        ballHit = Physics2D.CircleCast((Vector2)startPoint.position, 0.045f, -diff, diff.magnitude, 
                                        LayerMask.GetMask("balls", "edgecollider"));

        // dotted line and shading of dotted line for trajectory
        float width = trajectory.startWidth;
        trajectory.material.mainTextureScale = new Vector2(1.0f / width, 1.1f);
        trajectoryShade.material.mainTextureScale = new Vector2(1.0f / width, 1.1f);

        // pulling back the circlecast slightly to prevent linerenderer from going into circle prediction graphic
        Vector2 endCirclePoint = Vector2.MoveTowards(ballHit.point, cueBall.position, 0.045f);
        Vector2 endLinePoint = Vector2.MoveTowards(endCirclePoint, cueBall.position, 0.045f);
        Vector2 endLinePointShade = (Vector3)endLinePoint + (startPointShade.position - startPoint.position);

        // show projection of where cue ball will hit
        trajCircleTransform.position = endCirclePoint;
        
        // get ScreenPoint positions of cueBall and circlecast hit point
        var cueBallScreenPoint = Camera.main.WorldToScreenPoint(cueBall.position);
        var ballHitScreenPoint = Camera.main.WorldToScreenPoint(ballHit.point);

        // set positions of trajectory linerenderer
        trajectory.SetPosition(0, startPoint.position);
        trajectory.SetPosition(1, endLinePoint);
        // set positions of trajectory linerenderer shading (just a second line darker than the trajectory line)
        trajectoryShade.SetPosition(0, Vector2.MoveTowards(startPoint.position, startPointShade.position, 0.1f));
        trajectoryShade.SetPosition(1, Vector2.MoveTowards(endLinePoint, endLinePointShade, 0.1f));

        // if the player is aiming towards the right of the cueball
        if (ballHitScreenPoint.x >= cueBallScreenPoint.x) {
            // basically flip the lines so that the shading is on the other side of the trajectory line
            trajectory.SetPosition(0, Vector2.MoveTowards(startPoint.position, startPointShade.position, 0.1f));
            trajectory.SetPosition(1, Vector2.MoveTowards(endLinePoint, endLinePointShade, 0.1f));
            trajectoryShade.SetPosition(0, startPoint.position);
            trajectoryShade.SetPosition(1, endLinePoint);
        }
    }

    // function to enable the trajectory lines and circle
    void EnableTrajectory() {
        trajectory.gameObject.SetActive(true);
        trajectoryShade.gameObject.SetActive(true);
        trajectoryCircle.SetActive(true);
    }

    // function to disable the trajectory lines and circle
    void DisableTrajectory() {
        trajectory.gameObject.SetActive(false);
        trajectoryShade.gameObject.SetActive(false);
        trajectoryCircle.SetActive(false);
    }

    // sphere gizmo i used to get an idea of the circlecast size
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(ballHit.point, 0.045f);
    }
}
