using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableButton : MonoBehaviour
{
    [Header ("config")]
    [SerializeField] bool isMummyButton = false;
    [SerializeField] bool isStarting = false;

    [Header ("sprites")]
    [SerializeField] GameObject mummyFrame;
    [SerializeField] GameObject defaultFrame;

    [SerializeField] GameObject default_state;
    [SerializeField] GameObject pressable_state;
    [SerializeField] GameObject failed_state;
    [SerializeField] GameObject successful_state;

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

        if(this.isStarting && this.manager.getSequencePos() == 0) {
            this.pressable_state.SetActive(true);
            this.default_state.SetActive(false);
        }
    }

    public void setMummyButtonStatus(bool mummyBSatus) {
        if(flashingFailed) {
            return;
        }
        this.isMummyButton = mummyBSatus;

        this.mummyFrame.SetActive(this.isMummyButton);
        this.defaultFrame.SetActive(!this.isMummyButton);

        this.default_state.SetActive(true);
        this.pressable_state.SetActive(false);
        this.failed_state.SetActive(false);
        this.successful_state.SetActive(false);
    }
    
    public bool getMummyStatus() {
        return this.isMummyButton;
    }

    public void flashFailed() {
        flashingFailed = true;


        default_state.SetActive(false);
        pressable_state.SetActive(false);
        failed_state.SetActive(true);
        successful_state.SetActive(false);

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

        default_state.SetActive(false);
        pressable_state.SetActive(true);
        failed_state.SetActive(false);
        successful_state.SetActive(false);

        Invoke(nameof(setMummyButtonStatus), .4f);
    }
}
