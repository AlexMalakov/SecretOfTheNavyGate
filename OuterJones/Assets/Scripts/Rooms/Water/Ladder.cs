using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform ladderExit;
    private Canal canal;

    private Transform playerTransform = null;

    public void init(Canal c) {
        this.canal = c;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.playerTransform = other.gameObject.transform;
        }
    }
    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.playerTransform = null;
        }
    }

    public void Update() {
        if(Input.GetKeyDown(KeyCode.Space) && playerTransform != null) {
            playerTransform.position = ladderExit.position;
            this.canal.onLadderUse();
        }
    }
}
