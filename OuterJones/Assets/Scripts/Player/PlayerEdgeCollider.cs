using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEdgeCollider : MonoBehaviour
{
    //ok so this object exists literally as a game object data container :'(

    private List<Canal> canalsColidingWith = new List<Canal>();
    private List<CanalBorderManager> bordersCollidingWith = new List<CanalBorderManager>();


    public void setCanalStatus(bool status, Canal c) {
        if(status) {
            canalsColidingWith.Add(c);
        } else {
            canalsColidingWith.Remove(c); 
        }
    }

    public void setBorderStatus(bool status, CanalBorderManager b) {
        Debug.Log("HI IM " + gameObject.name + " AND IM COLLIDING? " + status);
        if(status) {
            bordersCollidingWith.Add(b);
        } else {
            bordersCollidingWith.Remove(b); 
        }
    }

    public bool isCollidingWithCanal(Canal c) {
        return this.canalsColidingWith.Contains(c);
    }

    public bool isCollidingWithBorder(CanalBorderManager b) {
        return this.bordersCollidingWith.Contains(b);
    }
}
