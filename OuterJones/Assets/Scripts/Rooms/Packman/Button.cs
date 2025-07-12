using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour, PowerableObject
{
    [SerializeField] bool startingButton;
    [SerializeField] Wire nextWire;
    [SerializeField] bool isMummyButton = false;

    [SerializeField] GameObject mummySprite;
    [SerializeField] GameObject playerSprite;
    bool powered;

    private ButtonManager manager;

    public void init(ButtonManager bm) {
        this.manager = bm;

        this.setMummyButtonStatus(isMummyButton);
    }

    void OnTriggerEnter2D(Collider2D other) {

        //this is bad and should be rewritten some other time
        if(other.gameObject.GetComponent<Player>() != null && this.manager == null) { //no manager exists, so i am a normal button
            Debug.Log("NORMAL BUTTON PRESS!");

            return;
        }

        if((other.gameObject.GetComponent<Player>() != null && !this.isMummyButton) 
                    || (other.gameObject.GetComponent<Mummy>() != null && this.isMummyButton)) {
            if(this.powered) {
                this.powered = false;
                StartCoroutine(nextWire.followPath());
            } else {
                this.manager.canStartSequence(this);
            }
            
        }
    }

    public bool isStartingButton() {
        return this.startingButton;
    }

    public void reset() {
        this.powered = false;
    }

    public void onPowered() {
        this.powered = true;
    }

    public void setMummyButtonStatus(bool mummyBSatus) {
        this.isMummyButton = mummyBSatus;

        if(isMummyButton) {
            mummySprite.SetActive(true);
            playerSprite.SetActive(false);
        } else {
            mummySprite.SetActive(false);
            playerSprite.SetActive(true);
        }
    }
    
    public bool getMummyStatus() {
        return this.isMummyButton;
    }
}
