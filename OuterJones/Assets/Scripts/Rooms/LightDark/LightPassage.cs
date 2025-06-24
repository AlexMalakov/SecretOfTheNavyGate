using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPassage : MonoBehaviour
{

    public GameObject objWithCollider;
    public Sprite unblockedSprite;
    public Sprite blockedSprite

    public void informLighting(bool lit) {

        objWithCollider.SetActive(!lit);
        unblockedSprite.enabled = lit;
        blockedSprite.enabled = !lit;
        
    }
}
