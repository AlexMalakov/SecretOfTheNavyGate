using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBridge : MonoBehaviour
{
    [SerializeField] private string playerOnUpperLayer = "Enviroment";
    [SerializeField] private string playerOnBottomLayer = "FrontForeground";


    private bool playerLevel;

    //basically if u want to cross the bridge u have to hit a collider first
    public void notifyOfPlayer(bool isUpperLevel) {
        Renderer rend = GetComponent<Renderer>();
        if(playerLevel && !isUpperLevel) {
            rend.sortingLayerName = this.playerOnBottomLayer;
            this.GetComponent<Collider2D>().enabled = false; //there shouldn't be a collider keeping the player on the bridge
        } else if(!playerLevel && isUpperLevel) {
            rend.sortingLayerName = this.playerOnUpperLayer;
            this.GetComponent<Collider2D>().enabled = true; //there should be a collider keeping the player on the bridge
        }
    }
}
