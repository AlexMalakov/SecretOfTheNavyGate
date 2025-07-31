using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableButton : MonoBehaviour
{
    [Header ("config")]
    [SerializeField] bool isMummyButton = false;

    [Header ("sprites")]
    [SerializeField] GameObject mummySprite;
    [SerializeField] GameObject playerSprite;
    [SerialzieField] GameObject pressableSprite;
    [SerializeField] GameObject failedSprite;

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
        if((other.gameObject.GetComponent<Player>() != null && !this.isMummyButton) 
                    || (other.gameObject.GetComponent<Mummy>() != null && this.isMummyButton)) {
            
            this.manager.onBottonPress(this.puzzleID);
        }
    }

    public void setMummyButtonStatus() {
        this.setMummyButtonStatus(this.isMummyButton);
    }

    public void setMummyButtonStatus(bool mummyBSatus) {
        if(flashingFailed) {
            return;
        }
        this.isMummyButton = mummyBSatus;

        if(isMummyButton) {
            this.pressableSprite.SetActive(false);
            this.failedSprite.SetActive(false);
            mummySprite.SetActive(true);
            playerSprite.SetActive(false);
        } else {
            this.pressableSprite.SetActive(false);
            this.failedSprite.SetActive(false);
            mummySprite.SetActive(false);
            playerSprite.SetActive(true);
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
}
