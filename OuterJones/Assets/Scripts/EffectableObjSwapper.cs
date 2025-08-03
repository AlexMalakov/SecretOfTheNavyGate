using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectableObjSwapper : MonoBehaviour, Effectable
{
    [SerializeField] private GameObject startingObj;
    [SerializeField] private GameObject secondObj;

    [SerializeField] private bool toggleBetween;
    private bool firstActive = true;


    public void onEffect() {
        if(toggleBetween) {
            firstActive = !firstActive;
            startingObj.SetActive(firstActive);
            secondObj.SetActive(!firstActive);
        } else {
            startingObj.SetActive(false);
            secondObj.SetActive(true);
        }
    }
}
