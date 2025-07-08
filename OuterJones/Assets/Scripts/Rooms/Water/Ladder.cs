using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : Floodable
{
    [SerializeField] private Transform ladderExit;
    private Canal canal;

    private Transform playerTransform = null;
    private bool flooded = false;

    public void init(Canal c) {
        this.canal = c;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && !flooded) {
            this.playerTransform = other.gameObject.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && !flooded) {
            this.playerTransform = null;
        }
    }

    public void Update() {
        //i don't like this for the use of
        if(playerTransform != null && PlayerInput.getSpaceInput(this.transform, "climb ladder")) {
            playerTransform.position = ladderExit.position;
            this.canal.onLadderUse();
        }
    }

    public override void onFlood() {
        this.flooded = true;
    }

    public override void drainWater() {
        this.flooded = false;
    }
}
