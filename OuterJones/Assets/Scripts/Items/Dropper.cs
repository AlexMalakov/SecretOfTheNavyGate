using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropper : MonoBehaviour, Effectable
{
    //give the player another key
    [SerializeField] Item i;
    private bool hasItem = true;

    public void onEffect() {
        if(hasItem) {
            hasItem = false;
            FindObjectOfType<Inventory>().gainItem(i);
        }
    }


}
