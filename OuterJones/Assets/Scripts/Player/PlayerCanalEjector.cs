using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//bandaid solution for the fact that i keep getting stuck in canals
public class PlayerCanalEjector : MonoBehaviour
{

    
    private bool isCollidingWithBorder = false;


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.layer == 8) {
            this.isCollidingWithBorder = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.layer == 8) {
            this.isCollidingWithBorder = false;
        }
    }




    public bool isPlayerCollidingWithBorder() {
        return this.isCollidingWithBorder;
    }
}
