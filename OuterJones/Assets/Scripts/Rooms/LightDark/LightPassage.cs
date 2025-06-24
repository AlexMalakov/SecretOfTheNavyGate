using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPassage : MonoBehaviour
{

    public GameObject objWithCollider;
    public GameObject unblockedSprite;
    public GameObject blockedSprite;

    public void informLighting(bool lit) {

        objWithCollider.SetActive(!lit);
        unblockedSprite.SetActive(lit);
        blockedSprite.SetActive(!lit);

    }
}
