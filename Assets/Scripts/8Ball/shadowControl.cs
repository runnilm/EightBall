using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shadowControl : MonoBehaviour {
    // shadow positions (transform.position) should always be on the opposite side of 
    // their parent.transform.position's from the center of the worldspace.

    // if in top left quadrant of worldspace (-x, +y)
    //      -> shadow appears at parentPos.x - 0.1f, parentPos.y + 0.1f
    // if in top right quadrant of worldspace (+x, +y)
    //      -> shadow appears at parentPos.x + 0.1f, parentPos.y + 0.1f
    // if in bottom left quadrant of worldspace (-x, -y)
    //      -> shadow appears at parentPos.x - 0.1f, parentPos.y - 0.1f
    // if in bottom right quadrant of worldspace (+x, -y)
    //      -> shadow appears at parentPos.x + 0.1f, parentPos.y - 0.1f

    // todo: widen these quadrant separating lines, and make "center" larger
    // possibly could use five large colliders, box colliders for quadrants and
    // a circle collider for the center. the center collider would take precedence.
    // this would replace this entire system and be much easier to debug, since this
    // is almost impossible to read.
    // ****OverlapBoxAll/OverlapAreaAll would be better than colliders.****
    // if on left line
    //      -> shadow appears at parentPos.x - 0.1f, parentPos.y
    // if on upper line
    //      -> shadow appears at parentPos.x, parentPos.y + 0.1f
    // if on right line
    //      -> shadow appears at parentPos.x + 0.1f, parentPos.y
    // if on lower line
    //      -> shadow appears at parentPos.x, parentPos.y - 0.1f
    // if on center
    //      -> shadow appears at parentPos.x, parentPos.y

    public Vector2 centerOfScreen;

    [SerializeField]
    public GameObject[] shadeableObjs;

    private Collider2D[] BLColliders;
    private Collider2D[] BColliders;
    private Collider2D[] BRColliders;
    private Collider2D[] LColliders;
    private Collider2D[] CColliders;
    private Collider2D[] RColliders;
    private Collider2D[] TLColliders;
    private Collider2D[] TColliders;
    private Collider2D[] TRColliders;

    private Collider2D[][] AllColliders = new Collider2D[9][];

    void Start() {
        centerOfScreen = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width / 2, Screen.height / 2));

        // updates the positions of all shade-able objects' shadows' every 1/10 a second
        // this could be made more performance friendly by updating the shadows anytime
        // a shade-able object is detected moving, but the impact is virtually nothing anyways
        InvokeRepeating("updateShadows", 0, .1f);
    }

    // function to update the positions of the shadows
    void updateShadows() {
        // this portion of the code essentially splits up the game screen into 9 boxes,
        // and checks to see if any gameobjects are within those areas. (by checking their colliders)
        // if it finds a gameobject in its area, it places the gameobject's collider into the
        // corresponding array.

        BLColliders =  Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(.33f, .33f)));
        AllColliders[0] = BLColliders;

        BColliders =   Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(.33f, 0)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(.66f, .33f)));
        AllColliders[1] = BColliders;

        BRColliders =  Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(.66f, 0)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(1f, .33f)));
        AllColliders[2] = BRColliders;

        LColliders =   Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(0, .33f)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(.33f, .66f)));;
        AllColliders[3] = LColliders;

        CColliders =   Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(.33f, .33f)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(.66f, .66f)));
        AllColliders[4] = CColliders;

        RColliders =   Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(.66f, .33f)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(1f, .66f)));
        AllColliders[5] = RColliders;

        TLColliders =  Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(0, .66f)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(.33f, 1f)));
        AllColliders[6] = TLColliders;

        TColliders =   Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(.33f, .66f)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(.66f, 1f)));
        AllColliders[7] = TColliders;

        TRColliders =  Physics2D.OverlapAreaAll(Camera.main.ViewportToWorldPoint(new Vector2(.66f, .66f)),
                                                            Camera.main.ViewportToWorldPoint(new Vector2(1f, 1f)));
        AllColliders[8] = TRColliders;

        // splitting viewport
        //////////////////////////////////////////////////////////////
        //         (.33, 1) //         (.66, 1) //           (1, 1) //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        // (0, .66)         // (.33, .66)       // (.66, .66)       //
        //////////////////////////////////////////////////////////////
        //       (.33, .66) //       (.66, .66) //         (1, .66) //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        // (0, .33)         // (.33, .33)       // (.66, .33)       //
        //////////////////////////////////////////////////////////////
        //       (.33, .33) //       (.66, .33) //         (1, .33) //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        //                  //                  //                  //
        // (0,0)            // (.33, 0)         // (.66, 0)         //
        //////////////////////////////////////////////////////////////

        int index = -1;
        // iterate through every array (there is one array per box, so 9)
        foreach (Collider2D[] colliderArr in AllColliders) {
            index++;
            // for every collider (object) in the array
            foreach (Collider2D colliderCol in colliderArr) {
                // grab the gameobject the collider is attached to
                GameObject colliderObj = colliderCol.gameObject;
                // grab the position of that gameobject
                Vector3 objPos = colliderObj.transform.position;

                // create an array of possible shadow positions based on a specified offset from the
                // position of the current object
                Vector2[] newPosArr = new Vector2[9];
                Vector2 BL = new Vector2(objPos.x - 0.01f, objPos.y - 0.01f);   newPosArr[0] = BL;
                Vector2 B = new Vector2(objPos.x, objPos.y - 0.01f);            newPosArr[1] = B;
                Vector2 BR = new Vector2(objPos.x + 0.01f, objPos.y - 0.01f);   newPosArr[2] = BR;
                Vector2 L = new Vector2(objPos.x - 0.01f, objPos.y);            newPosArr[3] = L;
                Vector2 C = new Vector2(objPos.x, objPos.y);                    newPosArr[4] = C;
                Vector2 R = new Vector2(objPos.x + 0.01f, objPos.y);            newPosArr[5] = R;
                Vector2 TL = new Vector2(objPos.x - 0.01f, objPos.y + 0.01f);   newPosArr[6] = TL;
                Vector2 T = new Vector2(objPos.x, objPos.y + 0.01f);            newPosArr[7] = T;
                Vector2 TR = new Vector2(objPos.x + 0.01f, objPos.y + 0.01f);   newPosArr[8] = TR;

                // if the gameobject should have a shadow (the balls and cue are the only ones that should)
                if (colliderObj.layer == LayerMask.NameToLayer("balls") || colliderObj.layer == LayerMask.NameToLayer("cueball")
                                                                        || colliderObj.layer == LayerMask.NameToLayer("cue")) {
                    // update the position of the shadow (which is the first child of each object)
                    // based on the current index, which points to the correct box the gameobject is in (0->8)
                    colliderObj.transform.GetChild(0).position = newPosArr[index];
                }
            }
        }
    }

    // gizmos i used to visualize the box areas on the screen
    private void OnDrawGizmos() {
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(0, 0)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.33f, .33f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.33f, 0)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.66f, .33f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.66f, 0)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(1f, .33f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.33f, .66f)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.66f, .66f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(1f, .66f)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(0, .66f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(0, .33f)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.66f, .66f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(1f, 0)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(0, 1f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.33f, 1f)), .1f);
        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(.66f, 1f)), .1f);

        Gizmos.DrawSphere(Camera.main.ViewportToWorldPoint(new Vector2(1f, 1f)), .1f);

        Gizmos.DrawLine(Camera.main.ViewportToWorldPoint(new Vector2(.67f, .78f)),
                        Camera.main.ViewportToWorldPoint(new Vector2(.67f, .22f)));
    }
}
