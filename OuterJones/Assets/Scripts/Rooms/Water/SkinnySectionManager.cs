using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinnySectionManager : MonoBehaviour
{
    bool hasFloaties = false;
    
    public void onFloatiesAquired() {
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
