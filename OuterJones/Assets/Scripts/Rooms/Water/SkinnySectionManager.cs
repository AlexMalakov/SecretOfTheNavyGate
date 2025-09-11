using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnySectionManager : MonoBehaviour, ItemListener
{
    bool hasFloaties = false;
    bool flooded = false;

    public void Awake() {
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Floaties, this);
    }

    public void onItemEvent(bool itemStatus) {
        this.hasFloaties = true;
        this.GetComponent<CompositeCollider2D>().isTrigger = this.flooded;
    }

    public void onFlood() {
        this.flooded = true;
        if(this.hasFloaties) {
            this.GetComponent<CompositeCollider2D>().isTrigger = true;
        }
    }

    public void onDrain() {
        this.flooded = false;
        if(this.hasFloaties) {
            this.GetComponent<CompositeCollider2D>().isTrigger = false;
        }
    }
}
