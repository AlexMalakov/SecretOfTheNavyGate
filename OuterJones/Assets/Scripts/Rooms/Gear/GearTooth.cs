using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearTooth : MonoBehaviour, ItemListener
{

    private RotatingGear gParent;
    private int ID;
    private bool onTooth = false;

    public void init(RotatingGear gParent, int ID) {
        this.gParent = gParent;
        this.ID = ID;
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Floaties, this);
    }

    public void onItemEvent(bool itemStatus) {
        if(this.onTooth) {
            this.gParent.playerOnTooth(this);
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            this.onTooth = true;
            this.gParent.playerOnTooth(this);
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<PlayerController>() != null) {
            this.onTooth = false;
            this.gParent.playerOffTooth();
        }
    }

    public int getID() {
        return this.ID;
    }
}
