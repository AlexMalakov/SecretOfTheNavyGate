using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge :  Floodable
{
    [SerializeField] GameObject notFloodedSprite;
    [SerializeField] GameObject floodedSprite;

    private bool flooded = false;
    private Canal canal;

    private void Start() {
        notFloodedSprite.SetActive(true);
        floodedSprite.SetActive(false);
        List<Collider2D> overlapping = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, overlapping);

        foreach (Collider2D c in overlapping) {
            if(c.gameObject.GetComponent<Canal>() != null) {
                this.canal = c.gameObject.GetComponent<Canal>();
            }
        }

        this.GetComponent<Collider2D>().enabled = false;
    }

    public override void onFlood() {
        flooded = true;
        notFloodedSprite.SetActive(false);
        floodedSprite.SetActive(true);

        this.GetComponent<Collider2D>().enabled = true;
    }

    public override void drainWater() {
        this.GetComponent<Collider2D>().enabled = false;
        flooded = false;
        notFloodedSprite.SetActive(true);
        floodedSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && flooded) {
            Physics2D.IgnoreCollision(other, this.canal.GetComponents<Collider2D>()[0], true);
            Physics2D.IgnoreCollision(other, this.canal.GetComponents<Collider2D>()[1], true);

            foreach(PlayerEdgeCollider e in other.gameObject.GetComponent<Player>().getEdgeColliders()) {
                Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[0], true);
                Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[1], true);
            }
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && flooded) {
            Physics2D.IgnoreCollision(other, this.canal.GetComponents<Collider2D>()[0], false);
            Physics2D.IgnoreCollision(other, this.canal.GetComponents<Collider2D>()[1], false);
            
            foreach(PlayerEdgeCollider e in other.gameObject.GetComponent<Player>().getEdgeColliders()) {
                Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[0], false);
                Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[1], false);
            }
        }
    }
}
