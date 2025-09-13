using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge :  Floodable, ItemListener
{
    [SerializeField] GameObject notFloodedSprite;
    [SerializeField] GameObject floodedSprite;
    [SerializeField] GameObject railingColliders;

    [SerializeField] private bool flooded = false;

    private bool playerHasFloaties = false;
    private Canal canal;

    private void Awake() {
        notFloodedSprite.SetActive(!this.flooded);
        floodedSprite.SetActive(this.flooded);

        List<Collider2D> overlapping = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, overlapping);

        foreach (Collider2D c in overlapping) {
            if(c.gameObject.GetComponent<Canal>() != null) {
                this.canal = c.gameObject.GetComponent<Canal>();
            }
        }

        if(this.canal == null) {
            Debug.Log("COULD NOT FIND A CANAL");
        }

        this.GetComponent<Collider2D>().enabled = this.flooded;
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Floaties, this);
    }

    public void onItemEvent(bool itemStatus) {
        this.playerHasFloaties = itemStatus;
        railingColliders.SetActive(!this.playerHasFloaties && this.flooded);
    }

    public override void onFlood(bool fromSource) {
        this.flooded = true;
        notFloodedSprite.SetActive(false);
        floodedSprite.SetActive(true);
        railingColliders.SetActive(!this.playerHasFloaties && this.flooded);

        this.GetComponent<Collider2D>().enabled = true;
    }

    public override void drainWater() {
        this.GetComponent<Collider2D>().enabled = false;
        this.flooded = false;
        notFloodedSprite.SetActive(true);
        floodedSprite.SetActive(false);
        railingColliders.SetActive(!this.playerHasFloaties && this.flooded);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && flooded) {
            this.setCollision(other.GetComponent<Player>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && flooded) {
            this.setCollision(other.GetComponent<Player>(), false);
        }
    }


    private void setCollision(Player p, bool status) {
        this.canal.getWaterCollider().setBridgeStatus(status);
    }
}
