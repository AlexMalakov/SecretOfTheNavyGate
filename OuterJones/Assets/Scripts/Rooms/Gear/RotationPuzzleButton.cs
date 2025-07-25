using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzleButton : RotationPuzzleElement
{
    [SerializeField] private GameObject effObj;

    private RotationPuzzleManager manager;
    private int pressNum;

    [SerializeField] private GameObject pressedObj;
    [SerializeField] private GameObject notPressedObj;

    public void initButton(RotationPuzzleManager manager, int pressNum) {
        this.manager = manager;
        this.pressNum = pressNum;
    }

    public override void resetElement() {
        pressedObj.SetActive(false);
        notPressedObj.SetActive(true);
    }

    public void isPressed() {
        pressedObj.SetActive(true);
        notPressedObj.SetActive(false);
    }

    public int getButtonNum() {
        return this.pressNum;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
        }
    }
}
