using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private List<Room> deck;
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject closed;

    private bool opened = false;

    void OnTriggerEnter2D(Collider2D other) {
        if(opened) {
            return;
        }

        if(other.gameObject.GetComponent<Player>() != null) {
            opened = true;
            other.gameObject.GetComponent<Player>().addToDeck(this.deck);
            open.SetActive(true);
            closed.SetActive(false);
        }
    }

    public void reset() {
        this.opened = false;
        this.open.SetActive(false);
        this.closed.SetActive(true);
    }
}
