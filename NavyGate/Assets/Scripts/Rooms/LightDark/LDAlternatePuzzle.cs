using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightPuzzleRoom {
    L3, L4, L5
}
public class LDAlternatePuzzle : MonoBehaviour
{
    [Header ("sprites")]
    [SerializeField] private GameObject l3LightSprite;
    [SerializeField] private GameObject l3DarkSprite;
    private bool l3On = false;

    [SerializeField] private GameObject l4LightSprite;
    [SerializeField] private GameObject l4DarkSprite;
    private bool l4On = false;

    [SerializeField] private GameObject l5LightSprite;
    [SerializeField] private GameObject l5DarkSprite;
    private bool l5On = false;

    [Header ("stairs")]
    [SerializeField] private UnderbellyStaircase staircase;
    bool activated = false;

    public void informLight(bool light, LightPuzzleRoom ldID) {

        if(this.activated) {
            return;
        }

        switch(ldID) {
            case LightPuzzleRoom.L3:
                this.l3On = light;
                this.l3LightSprite.SetActive(this.l3On);
                this.l3DarkSprite.SetActive(!this.l3On);
                break;

            case LightPuzzleRoom.L4:
                this.l4On = light;
                this.l4LightSprite.SetActive(this.l4On);
                this.l4DarkSprite.SetActive(!this.l4On);
                break;

            case LightPuzzleRoom.L5:
                this.l5On = light;
                this.l5LightSprite.SetActive(this.l5On);
                this.l5DarkSprite.SetActive(!this.l5On);
                break;
        }

        if(this.l3On && this.l4On && this.l5On) {
            this.staircase.onEffect();
            this.activated = true;
        }
    }
}
