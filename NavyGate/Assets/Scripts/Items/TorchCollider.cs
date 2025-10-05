using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchCollider : MonoBehaviour
{
    private bool isActive;

    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponentInParent<Mirror>() != null) {
            other.GetComponentInParent<Mirror>().clearWebs();
        }

        if(other.GetComponentInParent<CobwebDoor>() != null) {
            other.GetComponentInParent<CobwebDoor>().clearWebs();
        }
    }

    public void setActiveStatus(bool act) {
        this.isActive = act;
    }

}
