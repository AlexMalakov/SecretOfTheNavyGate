using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] Item i;
    [SerializeField] ObjListener keyListener;
    private bool hasItem = true;

    void OnTriggerEnter2D(Collider2D other) {
        if(hasItem && other.GetComponent<Player>() != null) {
            this.hasItem = false;
            other.GetComponent<Player>().getInventory().gainItem(i);

            if(this.keyListener != null) {
                this.keyListener.onStatusChanged(true);
            }

            this.gameObject.SetActive(false);
        }
    }
}
