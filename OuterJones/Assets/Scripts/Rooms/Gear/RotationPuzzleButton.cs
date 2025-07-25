using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzleButton : RotationPuzzleElement
{
    [SerializeField] private GameObject effObj;

    private RotationPuzzleManager manager;
    private int pressNum;




    public void initButton(RotationPuzzleManager manager, int pressNum) {
        this.manager = manager;
        this.pressNum = pressNum;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
        }
    }
}
