using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationListener : MonoBehaviour
{
    private int totalRotations = 0;
    [SerializeField] private List<GameObject> rotationSprites;
    [SerializeField] private GameObject effectableTarget;

    public void onRotation() {
        totalRotations++;

        if(totalRotations <= rotationSprites.Count - 1) {
            rotationSprites[totalRotations-1].SetActive(false);
            rotationSprites[totalRotations].SetActive(true);
        }

        if(totalRotations == rotationSprites.Count) {
            effectableTarget.GetComponent<Effectable>().onEffect();
        }

        
        
    }
}
