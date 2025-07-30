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

    [Header ("for puzzles")]
    [SerializeField] string puzzleID;
    // [SerializeField] Wire nextWire;

    private ButtonManager manager;

    public void init(ButtonManager bm) {
        this.manager = bm;

        this.setMummyButtonStatus(isMummyButton);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if((other.gameObject.GetComponent<Player>() != null && !this.isMummyButton) 
                    || (other.gameObject.GetComponent<Mummy>() != null && this.isMummyButton)) {
            
            this.manager.onBottonPress(this.puzzleID);
        }
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
