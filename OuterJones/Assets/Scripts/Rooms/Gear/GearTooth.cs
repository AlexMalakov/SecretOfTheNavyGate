using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTooth : MonoBehaviour
{

    private RotatingGear gParent;
    private int ID;

    public void init(RotatingGear gParent, int ID) {
        this.gParent = gParent;
        this.ID = ID;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            gParent.playerOnTooth(this, other.GetComponent<PlayerController>());
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            gParent.playerOffTooth();
        }
    }

    public int getID() {
        return this.ID;
    }
}
