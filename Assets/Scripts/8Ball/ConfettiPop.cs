using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfettiPop : MonoBehaviour {
    public ParticleSystem confetti;

    [SerializeField]
    public soundController soundController;

    // when a ball goes into a goal
    private void OnTriggerEnter2D(Collider2D collision) {
        // grab the position of the collision
        Vector2 confettiPos = collision.transform.position;

        // set the position of the confetti to the collision's position
        confetti.transform.position = confettiPos;

        // as long as the collider is not the cueball
        if (collision.gameObject.tag != "cueball") {
            // play the confetti burst effect
            confetti.Play();
            // play the confetti burst sound effect
            soundController.PlaySound(soundController.Sound.confettiPop, transform.position);
        }
    }
}
