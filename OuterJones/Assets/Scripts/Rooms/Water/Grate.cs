using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grate : MonoBehaviour
{
    [SerializeField] string enviromentLayer = "Enviroment";
    [SerializeField] string foregroundLayer = "FrontForeground";

    private Renderer rend;
    
    private bool playerInCanal = false;
    private Canal canal;


    private void Start() {
        List<Collider2D> overlapping = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(GetComponent<Collider2D>(), filter, overlapping);

        foreach (Collider2D c in overlapping) {
            if(c.gameObject.GetComponent<Canal>() != null) {
                this.canal = c.gameObject.GetComponent<Canal>();
            }
        }

        rend = GetComponent<Renderer>();
        rend.sortingLayerName = this.enviromentLayer;
    }


    public void onPlayerInCanal() {
        rend.sortingLayerName = this.foregroundLayer;
        this.playerInCanal = true;
    }

    public void onPlayerOutCanal() {
        rend.sortingLayerName = this.enviromentLayer;
        this.playerInCanal = false;

        
    }


    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("MIGHT IT!");
        if(other.gameObject.GetComponent<Player>() != null && !playerInCanal) {
            Debug.Log("MADE IT!");
            this.setCollision(other.GetComponent<Player>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        Debug.Log("GG!");
        if(other.gameObject.GetComponent<Player>() != null && !playerInCanal) {
            this.setCollision(other.GetComponent<Player>(), false);
        }
    }

    private void setCollision(Player p, bool status) {
        Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[0], status);
        Physics2D.IgnoreCollision(p.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[1], status);
        
        foreach(PlayerEdgeCollider e in p.getEdgeColliders()) {
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[0], status);
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.GetComponents<Collider2D>()[1], status);
        }
    }
}
