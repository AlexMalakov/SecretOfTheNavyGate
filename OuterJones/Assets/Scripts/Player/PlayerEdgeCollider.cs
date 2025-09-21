using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEdgeCollider : MonoBehaviour
{
    //ok so this object exists literally as a game object data container :'(

    private List<Canal> canalsColidingWith = new List<Canal>();


    public void setCanalStatus(bool status, Canal c) {
        if(status) {
            canalsColidingWith.Add(c);
        } else {
            canalsColidingWith.Remove(c); 
        }
    }

    public bool isCollidingWithCanal(Canal c) {
        return this.canalsColidingWith.Contains(c);
    }
}
