using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCanalFinders : MonoBehaviour
{
    //ok so this object exists literally as a game object data container :'(


    private Canal colidingWith;


    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Canal>() != null) {
            this.colidingWith = other.GetComponent<Canal>();
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Canal>() != null) {
            this.colidingWith = null;
        }
    }

    public bool collidingWithCanal(Canal c) {
        return colidingWith == c;
    }
}
