using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, PowerableObject
{
    [SerializeField] bool startingButton;
    [SerializeField] Wire nextWire;
    bool powered;

    private ButtonManager manager;

    public void init(ButtonManager bm) {
        this.manager = bm;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null || other.gameObject.GetComponent<Mummy>() != null) {
            if(this.powered) {
                this.powered = false;
                StartCoroutine(nextWire.followPath());
            } else {
                this.manager.canStartSequence(this);
            }
            
        }
    }

    public bool isStartingButton() {
        return this.startingButton;
    }

    public void reset() {
        this.powered = false;
    }

    public void onPowered() {
        this.powered = true;
    }
}
