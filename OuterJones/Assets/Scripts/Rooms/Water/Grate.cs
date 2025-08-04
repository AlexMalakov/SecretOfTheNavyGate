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
