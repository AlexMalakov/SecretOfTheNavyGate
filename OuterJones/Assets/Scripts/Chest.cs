using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] private List<Room> deck;
    [SerializeField] private GameObject open;
    [SerializeField] private GameObject closed;

    private bool opened = false;

    public IEnumerator Start() {
        yield return null;
        yield return null; //skips 2 frames before hiding every room
        foreach(Room r in this.deck) { //IM SETTING EVERY ROOM TO NOT ACTIVE!
            r.gameObject.SetActive(false);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(opened) {
            return;
        }

        if(other.gameObject.GetComponent<Player>() != null) {
            opened = true;
            other.gameObject.GetComponent<Player>().addToDeck(this.deck);
            open.SetActive(true);
            closed.SetActive(false);

            FindObjectOfType<DeckUI>().onUpdate();
        }
    }

    public void reset() {
        this.opened = false;
        this.open.SetActive(false);
        this.closed.SetActive(true);
    }
}
