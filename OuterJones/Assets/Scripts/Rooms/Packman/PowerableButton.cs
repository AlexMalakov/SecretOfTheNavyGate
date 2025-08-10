using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableButton : MonoBehaviour
{
    [Header ("config")]
    [SerializeField] bool isMummyButton = false;
    [SerializeField] bool isStarting = false;

    [Header ("sprites")]
    [SerializeField] GameObject mummySprite;
    [SerializeField] GameObject playerSprite;
    [SerializeField] GameObject pressableSprite;
    [SerializeField] GameObject failedSprite;
    [SerializeField] GameObject starting;

    [Header ("for puzzles")]
    [SerializeField] string puzzleID;

    private bool flashingFailed = false;
    // [SerializeField] Wire nextWire;

    private ButtonManager manager;

    public void init(ButtonManager bm) {
        this.manager = bm;

        this.setMummyButtonStatus();
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("HIT" + other.gameObject.name);
        if((other.gameObject.GetComponent<Player>() != null && !this.isMummyButton) 
                    || (other.gameObject.GetComponent<Mummy>() != null && this.isMummyButton)) {
            
            this.manager.onBottonPress(this.puzzleID);
        }
    }

    public void setMummyButtonStatus() {
        this.setMummyButtonStatus(this.isMummyButton);

        if(this.isStarting) {
            this.starting.SetActive(true);
            this.mummySprite.SetActive(false);
            this.playerSprite.SetActive(false);
        }
    }

    public void setMummyButtonStatus(bool mummyBSatus) {
        if(flashingFailed) {
            return;
        }
        this.isMummyButton = mummyBSatus;

        if(isMummyButton) {
            this.pressableSprite.SetActive(false);
            this.failedSprite.SetActive(false);
            this.mummySprite.SetActive(true);
            this.playerSprite.SetActive(false);
            this.starting.SetActive(false);
        } else {
            this.pressableSprite.SetActive(false);
            this.failedSprite.SetActive(false);
            this.mummySprite.SetActive(false);
            this.playerSprite.SetActive(true);
            this.starting.SetActive(false);
        }
    }
    
    public bool getMummyStatus() {
        return this.isMummyButton;
    }

    public void flashFailed() {
        flashingFailed = true;
        this.playerSprite.SetActive(false);
        this.mummySprite.SetActive(false);
        this.pressableSprite.SetActive(false);
        this.failedSprite.SetActive(true);

        Invoke(nameof(unflashFailed), .7f);
    }

    private void unflashFailed() {
        this.flashingFailed = false;
        setMummyButtonStatus();
    }

    public void flashPressable() {
        if(this.flashingFailed) {
            return;
        }

        this.playerSprite.SetActive(false);
        this.mummySprite.SetActive(false);
        this.pressableSprite.SetActive(true);
        Invoke(nameof(setMummyButtonStatus), .4f);
    }

    public void setStarting() {
        this.playerSprite.SetActive(false);
        this.mummySprite.SetActive(false);
        this.starting.SetActive(true);
    }
}
