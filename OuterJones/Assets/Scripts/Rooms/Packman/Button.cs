using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    private PackmanRoom room;

    public void init(PackmanRoom r) {
        this.room = r;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null || other.gameObject.GetComponent<Mummy>() != null) {
            this.room.onButtonEvent(this, true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null || other.gameObject.GetComponent<Mummy>() != null) {
            this.room.onButtonEvent(this, false);
        }
    }
}
