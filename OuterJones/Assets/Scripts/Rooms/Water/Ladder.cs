using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    [SerializeField] private Transform ladderExit;
    private Canal canal;

    public void init(Canal c) {
        this.canal = c;
    }

    void OnTriggerStay2D(Collider2D other) {
        if(Input.GetKeyDown(KeyCode.Space) && other.gameObject.GetComponent<Player>() != null) {
            other.gameObject.GetComponent<Player>().transform.position = ladderExit.position;
            this.canal.onLadderUse();
        }
    }
}
