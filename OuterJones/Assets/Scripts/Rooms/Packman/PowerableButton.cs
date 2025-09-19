using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerableButton : MonoBehaviour, ItemListener
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

    private bool collidingWithPlayer = false;
    private bool collidingWithMummy = false;

    private ButtonManager manager;

    private void Awake() {
        this.setMummyButtonStatus();
        FindObjectOfType<Inventory>().addItemListener(PossibleItems.Amulet, this);
    }

    public void init(ButtonManager bm) {
        this.manager = bm;
        this.resetToDefault();
    }
        

    public void onItemEvent(bool itemStatus) {
        this.setMummyButtonStatus(!this.isMummyButton);
        
        if((this.isMummyButton && this.collidingWithMummy) || (!this.isMummyButton && this.collidingWithPlayer)) {
            this.manager.onBottonPress(this.puzzleID);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        this.collidingWithPlayer = this.collidingWithPlayer || (other.gameObject.GetComponent<Player>() != null);
        this.collidingWithMummy = this.collidingWithMummy || (other.gameObject.GetComponent<Mummy>() != null);

        if((other.gameObject.GetComponent<Player>() != null && !this.isMummyButton) 
                    || (other.gameObject.GetComponent<Mummy>() != null && this.isMummyButton)) {
            
            this.manager.onBottonPress(this.puzzleID);
        }
    }

    void OnTriggerExit2D(Collider2D other) {
        this.collidingWithPlayer = this.collidingWithPlayer && (!(other.gameObject.GetComponent<Player>() != null));
        this.collidingWithMummy = this.collidingWithMummy && (!(other.gameObject.GetComponent<Mummy>() != null));
    }

    public void setMummyButtonStatus() {
        this.setMummyButtonStatus(this.isMummyButton);
    }

    public void setMummyButtonStatus(bool mummyBSatus) {
        this.isMummyButton = mummyBSatus;
        this.mummyFrame.SetActive(this.isMummyButton);
        this.defaultFrame.SetActive(!this.isMummyButton);

        if(flashingFailed) {
            return;
        }
    }

    private void resetToDefault() {
        this.default_state.SetActive(true);
        this.pressable_state.SetActive(false);
        this.failed_state.SetActive(false);
        this.successful_state.SetActive(false);

        if(this.isStarting && this.manager.getSequencePos() == 0) {
            this.pressable_state.SetActive(true);
            this.default_state.SetActive(false);
        }
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
        this.resetToDefault();
    }

    public void flashPressable() {
        if(this.flashingFailed) {
            return;
        }

        default_state.SetActive(false);
        pressable_state.SetActive(true);
        failed_state.SetActive(false);
        successful_state.SetActive(false);

        Invoke(nameof(resetToDefault), .4f);
    }

    public void flashSuccess() {
        default_state.SetActive(false);
        pressable_state.SetActive(false);
        failed_state.SetActive(false);
        successful_state.SetActive(true);

        Invoke(nameof(resetToDefault), .7f);
    }
}
