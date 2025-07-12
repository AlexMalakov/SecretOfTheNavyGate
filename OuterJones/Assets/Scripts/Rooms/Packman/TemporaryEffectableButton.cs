using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//button is currently a terrible script thats desgined to exist in only packman rooms
//so this should be more abstract
public class TemporaryEffectableButton : MonoBehaviour
{
    [SerializeField] private GameObject effObj;


    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
        }
    }
}
