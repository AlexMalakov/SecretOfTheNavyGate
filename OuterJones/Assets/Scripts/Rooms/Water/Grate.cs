using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grate : MonoBehaviour
{
    [SerializeField] private string enviromentLayer = "Enviroment";
    [SerializeField] private string foregroundLayer = "FrontForeground";

    [SerializeField] private Collider2D outOfCanalCollider;

    [SerializeField] private Renderer rend;
    
    private bool playerInCanal = false;
    private Canal canal;


    private void Awake() {
        List<Collider2D> overlapping = new List<Collider2D>();
        ContactFilter2D filter = new ContactFilter2D();
        filter.useTriggers = true;
        Physics2D.OverlapCollider(GetComponent<CompositeCollider2D>(), filter, overlapping);

        foreach (Collider2D c in overlapping) {
            if(c.gameObject.GetComponent<Canal>() != null) {
                this.canal = c.gameObject.GetComponent<Canal>();
            }
        }

        if(this.canal == null) {
            Debug.Log("COULD NOT FIND CANAL!");
        } else {
            Debug.Log("COULD FIND CANAL!");
        }

        this.rend.sortingLayerName = this.enviromentLayer;
        this.outOfCanalCollider.enabled = true;
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
            Debug.Log("BLOCKED COLLSIONS AT" + Time.time);
            this.setCollision(other.GetComponent<CanalFinderManager>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<CanalFinderManager>() != null && !playerInCanal) {
            this.setCollision(other.GetComponent<CanalFinderManager>(), false);
        }
    }

    private void setCollision(CanalFinderManager c, bool status) {
        Physics2D.IgnoreCollision(c.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[0], status);
        Physics2D.IgnoreCollision(c.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[1], status);
        
        foreach(PlayerEdgeCollider e in c.GetComponentInParent<Player>().getEdgeColliders()) {
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[0], status);
            Physics2D.IgnoreCollision(e.GetComponent<Collider2D>(), this.canal.getWaterCollider().GetComponents<Collider2D>()[1], status);
        }
    }
}
