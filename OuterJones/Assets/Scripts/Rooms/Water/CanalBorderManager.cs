using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanalBorderManager : Floodable
{
    private bool isFlooded;


    public override void onFlood(bool fromSource) {
        this.isFlooded = true;
    }

    public override void drainWater() {
        this.isFlooded = false;
    }

    public bool isBorderActive() {
        return this.isFlooded;
    }

    public void OnCollisionEnter2D(Collision2D col) {
        Debug.Log("???? COLLIDNG? " + col.collider.gameObject.name);
        if(col.collider.GetComponent<PlayerEdgeCollider>() != null && !isFlooded) {
            col.collider.GetComponent<PlayerEdgeCollider>().setBorderStatus(true, this);
        }
    }

    public void OnCollisionExit2D(Collision2D col) {
        if(col.collider.GetComponent<PlayerEdgeCollider>() != null) {
            col.collider.GetComponent<PlayerEdgeCollider>().setBorderStatus(false, this);
        }
    }
}
