using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEdgeCollider : MonoBehaviour
{
    //ok so this object exists literally as a game object data container :'(


    private bool isCollidingCanal;


    public void setCanalStatus(bool status) {
        this.isCollidingCanal = status;
    }

    public bool isCollidingWithCanal() {
        return this.isCollidingCanal;
    }
}
