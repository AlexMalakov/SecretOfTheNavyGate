using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bushes : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            other.GetComponent<Player>().setPlayerInBush(true);
        }
    }

    public void OnTriggerExit2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            other.GetComponent<Player>().setPlayerInBush(true);
        }
    }
}
