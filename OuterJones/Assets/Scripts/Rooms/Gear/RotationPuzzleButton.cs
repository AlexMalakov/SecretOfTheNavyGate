using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPuzzleButton : MonoBehaviour, RotationPuzzleElement
{
    [SerializeField] private GameObject effObj;

    public void init(RotationPuzzleManager manager) {

    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.GetComponent<Player>() != null) {
            this.effObj.GetComponent<Effectable>().onEffect();
        }
    }
}
