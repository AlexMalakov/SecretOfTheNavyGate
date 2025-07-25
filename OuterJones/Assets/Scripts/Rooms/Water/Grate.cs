using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grate : MonoBehaviour
{
    [SerializeField] private string enviromentLayer = "Enviroment";
    [SerializeField] private string foregroundLayer = "FrontForeground";

    [SerializeField] private CompositeCollider2D outOfCanalCollider;

    [SerializeField] private Renderer rend;
    
    private bool playerInCanal = false;
    private bool playerOnGrate = false;
    private Canal canal;

    public void init(Canal c) {
        this.rend.sortingLayerName = this.enviromentLayer;
        this.outOfCanalCollider.enabled = true;
        this.canal = c;
    }


    public void onPlayerInCanal() {
        rend.sortingLayerName = this.foregroundLayer;
        this.playerInCanal = true;

        this.outOfCanalCollider.enabled = false;
        
    }

    public void onPlayerOutCanal() {
        rend.sortingLayerName = this.enviromentLayer;
        this.playerInCanal = false;

        this.outOfCanalCollider.enabled = true;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<CanalFinderManager>() != null && !playerInCanal) {
            // this.setCollision(other.GetComponent<CanalFinderManager>(), true);
            this.playerOnGrate = true;
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<CanalFinderManager>() != null && !playerInCanal) {
            // this.setCollision(other.GetComponent<CanalFinderManager>(), false);
            this.playerOnGrate = false;
        }
    }

    private void setCollision(CanalFinderManager c, bool status) {
        this.playerOnGrate = status;
        Physics2D.IgnoreCollision(c.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[0], status);
        Physics2D.IgnoreCollision(c.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[1], status);
        
        foreach(PlayerEdgeCollider e in c.GetComponentInParent<Player>().getEdgeColliders()) {
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[0], status);
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[1], status);
        }
    }

    public bool isPlayerOnGrate() {
        return this.playerOnGrate;
    }
}
