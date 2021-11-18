using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ffHint : MonoBehaviour {
    public Cueball cb;

    private void Update() {
        // if the cueball has been moving for 5+ seconds, should remind player that they can fastforward
        if (cb.timeMoving >= 5f) {
            // start hint animation
            GetComponent<Animator>().SetBool("shouldHint", true);
        } else {
            // end hint animation
            GetComponent<Animator>().SetBool("shouldHint", false);
        }
    }

}
