using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationListener : MonoBehaviour
{
    private int totalRotations = 0;
    private bool direction = true;
    [SerializeField] private List<GameObject> rotationSprites;
    [SerializeField] private GameObject effectableTarget;

    public void onRotation(bool clockwise) {
        if(clockwise == !this.direction) {
            this.direction = clockwise;

            if(this.totalRotations <= rotationSprites.Count - 1) {
                rotationSprites[this.totalRotations].SetActive(false);
            } else {
                rotationSprites[rotationSprites.Count - 1].SetActive(false);
            }
            
            this.totalRotations = 1;
        } else {
            this.totalRotations++;
        }

        if(this.totalRotations <= rotationSprites.Count - 1) {
            rotationSprites[this.totalRotations-1].SetActive(false);
            rotationSprites[this.totalRotations].SetActive(true);
        }

        if(totalRotations == rotationSprites.Count) {
            effectableTarget.GetComponent<Effectable>().onEffect();
        }

        
        
    }
}
