using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class musicPlayer : MonoBehaviour {
    private static musicPlayer instance = null;

    public static musicPlayer Instance {
        get {
            return instance;
        }
    }

    private void Awake() {
        // essentially makes a singleton of the background music playing
        if (instance != null && instance != this) {
            Destroy(this.gameObject);
            return;
        } else {
            instance = this;
        }
        // music playing persists through the scene being restarted
        DontDestroyOnLoad(this.gameObject);
    }

}
