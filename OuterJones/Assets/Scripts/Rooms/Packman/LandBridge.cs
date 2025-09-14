using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandBridge : MonoBehaviour
{
    [SerializeField] private GameObject playerTravelingAbove;
    [SerializeField] private GameObject playerTravelingBelow;


    [SerializeField] private bool playerLevelIsUpper = true;

    //basically if u want to cross the bridge u have to hit a collider first
    public void notifyOfPlayer(bool isUpperLevel) {
        if(playerLevelIsUpper && !isUpperLevel) {
            this.playerTravelingAbove.SetActive(false);
            this.playerTravelingBelow.SetActive(true);
            playerLevelIsUpper = false;
        } else if(!playerLevelIsUpper && isUpperLevel) {
            this.playerTravelingAbove.SetActive(true);
            this.playerTravelingBelow.SetActive(false);
            playerLevelIsUpper = true;
        }
    }
}
