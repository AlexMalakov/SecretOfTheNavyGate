using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour
{
    [SerializeField] Item i;
    private bool hasItem = true;

    void OnTriggerEnter2D(Collider2D other) {
        if(hasItem && other.GetComponent<Player>() != null) {
            this.hasItem = false;
            other.GetComponent<Player>().getInventory().gainItem(i);
        }
    }
}
