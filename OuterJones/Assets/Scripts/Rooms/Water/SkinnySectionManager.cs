using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnySectionManager : MonoBehaviour, ItemListener
{
    [SerializeField] private Inventory itemListenerManager;
    bool hasFloaties = false;

    public void Awake() {
        itemListenerManager.addItemListener(PossibleItems.Floaties, this);
    }

    public void onItemEvent(bool itemStatus) {
        this.hasFloaties = true;
    }

    public void onFlood() {
        if(this.hasFloaties) {
            this.GetComponent<CompositeCollider2D>().isTrigger = true;
        }
    }

    public void onDrain() {
        if(this.hasFloaties) {
            this.GetComponent<CompositeCollider2D>().isTrigger = false;
        }
    }
}
