using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchCollider : MonoBehaviour
{
    private bool isActive;

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Mirror>() != null) {
            other.GetComponent<Mirror>().clearWebs();
        }

        if(other.GetComponent<CobwebDoor>() != null) {
            other.GetComponent<CobwebDoor>().clearWebs();
        }
    }

    public void setActiveStatus(bool act) {
        this.isActive = act;
    }

}
