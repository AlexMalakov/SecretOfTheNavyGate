using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//button is currently a terrible script thats desgined to exist in only packman rooms
//so this should be more abstract
//ok i kinda dont want to rewrite it tho so maybe not so temporary after all >:)
public class TemporaryEffectableButton : MonoBehaviour, Effectable
{
    [SerializeField] private GameObject effObj;

    [SerializeField] private bool requiresActivation = false; 

    [SerializeField] private GameObject defaultSprite;
    [SerializeField] private GameObject pressedSprite;

    void Awake() {
        this.defaultSprite.SetActive(true);
        this.pressedSprite.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!this.requiresActivation && other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
            this.defaultSprite.SetActive(false);
            this.pressedSprite.SetActive(true);

            Invoke(nameof(resetSprite), .5f);
        }
    }

    public void onEffect() {
        this.requiresActivation = false;
    }

    private void resetSprite() {
        this.defaultSprite.SetActive(true);
        this.pressedSprite.SetActive(false);
    }
}
