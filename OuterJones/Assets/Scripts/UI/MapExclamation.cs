using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapExclamation : MonoBehaviour
{
    [SerializeField] private int totalFlashCount = -1;
    [SerializeField] private Image exclamationImage;

    [SerializeField] private Sprite redExclamation;
    [SerializeField] private Sprite yellowExclamation;
    private MapExclamationManager manager;

    int flashCount;
    private bool flashing = false;


    public void startFlash(RectTransform flashPos, bool countFlash) {
        this.flashing = true;
        this.flashCount = countFlash? this.totalFlashCount : -1;
        this.exclamationImage.sprite = countFlash? this.yellowExclamation : this.redExclamation;

        this.exclamationImage.GetComponent<RectTransform>().anchoredPosition = flashPos.anchoredPosition;
    }

    public void setManager(MapExclamationManager manager) {
        this.manager = manager;
    }

    public void flashExclamation() {
        if(!this.flashing || this.flashCount == 0) {
            this.flashing = false;
            manager.onExclamationOver(this);
            return;
        }

        this.exclamationImage.gameObject.SetActive(true);
    }

    public void unflashExclamation() {
        if(!this.flashing || this.flashCount == 0) {
            this.flashing = false;
            manager.onExclamationOver(this);
            return;
        }

        this.exclamationImage.gameObject.SetActive(false);

        if(this.flashCount > 0) {
            this.flashCount--;
        }
    }

    public bool isActive() {
        return this.flashing;
    }

    public void endEarly() {
        this.flashing = false;
    }
}
