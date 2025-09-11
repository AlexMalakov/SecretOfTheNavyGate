using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grate : MonoBehaviour, ItemListener
{
    [SerializeField] private string enviromentLayer = "Enviroment";
    [SerializeField] private string foregroundLayer = "FrontForeground";

    [SerializeField] private CompositeCollider2D outOfCanalCollider;

    [SerializeField] private Renderer rend;
    
    private bool playerInCanal = false;
    private bool playerOnGrate = false;
    private bool playerHasFloaties = false;
    private Canal canal;

    public void init(Canal c) {
        this.rend.sortingLayerName = this.enviromentLayer;
        this.outOfCanalCollider.isTrigger = false;
        this.canal = c;
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Floaties, this);
    }


    public void onPlayerInCanal() {
        rend.sortingLayerName = this.foregroundLayer;
        this.playerInCanal = true;

        this.outOfCanalCollider.isTrigger = true;
    }

    public void onItemEvent(bool itemStatus) {
        this.playerHasFloaties = itemStatus;
        outOfCanalCollider.enabled = this.playerHasFloaties || this.playerInCanal;
    }

    public void onPlayerOutCanal() {
        rend.sortingLayerName = this.enviromentLayer;
        this.playerInCanal = false;

        this.outOfCanalCollider.isTrigger = this.playerHasFloaties;
    }


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && !playerInCanal) {
            this.setCollision(other.GetComponent<Player>(), true);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null && !playerInCanal) {
            this.setCollision(other.GetComponent<Player>(), false);
        }
    }

    private void setCollision(Player p, bool status) {
        this.playerOnGrate = status;
        this.canal.getWaterCollider().setGrateStatus(status);
    }

    public bool isPlayerOnGrate() {
        return this.playerOnGrate;
    }
}
