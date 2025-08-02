using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleZone : MonoBehaviour
{

    private GraplePoint papa;
    private int id;

    public void init(GraplePoint papa, int id) {
        this.papa = papa;
        this.id = id;
    }
    
    void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.papa.grapleFrom(this.id);
        }
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            this.papa.pointExit();
        }
    }
}
