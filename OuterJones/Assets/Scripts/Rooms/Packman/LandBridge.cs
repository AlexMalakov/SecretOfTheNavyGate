using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBridge : MonoBehaviour
{
    [SerializeField] private string playerOnUpperLayer = "Enviroment";
    [SerializeField] private string playerOnBottomLayer = "FrontForeground";

    [SerializeField] private GameObject railingCollider;
    [SerializeField] private GameObject groundCollider;


    private bool playerLevel = true;

    //basically if u want to cross the bridge u have to hit a collider first
    public void notifyOfPlayer(bool isUpperLevel) {
        Renderer rend = GetComponent<Renderer>();
        if(playerLevel && !isUpperLevel) {
            rend.sortingLayerName = this.playerOnBottomLayer;
            this.railingCollider.SetActive(false); //there shouldn't be a collider keeping the player on the bridge
            this.groundCollider.SetActive(true);
        } else if(!playerLevel && isUpperLevel) {
            rend.sortingLayerName = this.playerOnUpperLayer;
            this.railingCollider.SetActive(true); //there should be a collider keeping the player on the bridge
            this.groundCollider.SetActive(false);
        }
    }
}
