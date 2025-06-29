using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, PowerableObject
{
    private PackmanRoom room;
    [SerializeField] bool canBePressed;

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

    public void onPowered() {
        this.canBePressed = true;
    }
}
