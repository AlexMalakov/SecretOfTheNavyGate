using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//button is currently a terrible script thats desgined to exist in only packman rooms
//so this should be more abstract
//ok i kinda dont want to rewrite it tho so maybe not so temporary after all >:)
public class TemporaryEffectableButton : MonoBehaviour, Effectable //ok update: code is finalized in a week this mfer permenent
{
    [SerializeField] private GameObject effObj;

    [SerializeField] private bool requiresActivation = false; 

    [SerializeField] private GameObject defaultSprite;
    [SerializeField] private GameObject pressedSprite;
    [SerializeField] private GameObject notActiveSprite;

    private Quaternion initialRot;

    void Awake() {
        this.resetSprite();
        this.initialRot = this.transform.rotation;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(!this.requiresActivation && other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
            this.defaultSprite.SetActive(false);
            this.notActiveSprite.SetActive(false);
            this.pressedSprite.SetActive(true);

            Invoke(nameof(resetSprite), .5f);
        }
    }

    public void rotate90() {
        this.transform.rotation = this.initialRot;
    }

    public void onEffect() {
        this.requiresActivation = false;
        this.resetSprite();
    }

    public void onEffectOver() {
        this.requiresActivation = true;
        this.resetSprite();
    }

    private void resetSprite() {
        this.defaultSprite.SetActive(!requiresActivation);
        this.notActiveSprite.SetActive(requiresActivation);
        this.pressedSprite.SetActive(false);
    }
}
