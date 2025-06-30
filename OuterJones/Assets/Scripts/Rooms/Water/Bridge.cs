using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bridge : MonoBehaviour, Floodable
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

    public void onFlood() {
        flooded = true;
        notFloodedSprite.SetActive(false);
        floodedSprite.SetActive(true);

        this.GetComponent<Collider2D>().enabled = true;
    }

    public void drainWater() {
        this.GetComponent<Collider2D>().enabled = false;
        flooded = false;
        notFloodedSprite.SetActive(true);
        floodedSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && flooded) {
            // Collider2D playerCollider = other.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(other, this.canal.GetComponent<Collider2D>(), true);
        }   
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && flooded) {
            // Collider2D playerCollider = other.GetComponent<Collider2D>();
            Physics2D.IgnoreCollision(other, this.canal.GetComponent<Collider2D>(), false);
        }
    }
}
