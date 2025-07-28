using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightPuzzleRoom {
    L1, L2, L3
}
public class LDAlternatePuzzle : MonoBehaviour
{
    [Header ("sprites")]
    [SerializeField] private GameObject l1LightSprite;
    [SerializeField] private GameObject l1DarkSprite;
    private bool l1On = false;

    [SerializeField] private GameObject l2LightSprite;
    [SerializeField] private GameObject l2DarkSprite;
    private bool l2On = false;

    [SerializeField] private GameObject l3LightSprite;
    [SerializeField] private GameObject l3DarkSprite;
    private bool l3On = false;

    [Header ("stairs")]
    [SerializeField] private UnderbellyStaircase staircase;
    bool activated = false;

    public void informLight(bool light, LightPuzzleRoom ldID) {

        if(this.activated) {
            return;
        }

        switch(ldID) {
            case LightPuzzleRoom.L1:
                this.l1On = !light;
                this.l1LightSprite.SetActive(this.l1On);
                this.l1DarkSprite.SetActive(!this.l1On);
                break;

            case LightPuzzleRoom.L2:
                this.l2On = !light;
                this.l2LightSprite.SetActive(this.l2On);
                this.l2DarkSprite.SetActive(!this.l2On);
                break;

            case LightPuzzleRoom.L3:
                this.l3On = !light;
                this.l3LightSprite.SetActive(this.l3On);
                this.l3DarkSprite.SetActive(!this.l3On);
                break;
        }

        if(this.l1On && this.l2On && this.l3On) {
            this.staircase.onEffect();
            this.activated = true;
        }
    }
}
