using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//button is currently a terrible script thats desgined to exist in only packman rooms
//so this should be more abstract
public class TemporaryEffectableButton : MonoBehaviour, Effectable
{
    [SerializeField] private GameObject effObj;

    [SerializeField] private bool requiresActivation = false; 


    void OnTriggerEnter2D(Collider2D other) {
        if(!this.requiresActivation && other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
        }
    }

    public void onEffect() {
        this.requiresActivation = false;
    }
}
