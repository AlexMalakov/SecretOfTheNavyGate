using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyDoor : GateDoor, InputSubscriber
{
    private PlayerIO inputManager;
    private Player player;

    protected override void Awake() {
        base.Awake();
        this.inputManager = FindObjectOfType<PlayerIO>();
        this.player = FindObjectOfType<Player>();
    }

    public void OnTriggerEnter2D(Collider2D other) {
        if(this.openState) {
            return;
        }

        if(other.GetComponent<Player>() != null && other.GetComponent<Player>().hasKey()) {
            inputManager.requestSpaceInput(this, this.transform, "open door with key");
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            inputManager.cancelRequest(this);
        }
    }

    public void onSpacePress() {
        player.useKey();
        this.toggleOpen();
    }
}