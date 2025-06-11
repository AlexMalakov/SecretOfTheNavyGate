using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Room> deck;


    public void Start() {
        deck = new List<Room>();

        GameObject obj = GameObject.Find("deckRoom");
        deck.Add(obj.GetComponent<Room>());
    }

    public void addToDeck(List<Room> newDeck) {
        this.deck.AddRange(newDeck);
    }

    public Room getNextInDeck() {
        if(deck.Count == 0)
            return null;
        return deck[0];
    }

    //throws out of bounds exception, should never occur tho so it going unhandled is best
    public void removeNextInDeck() {
        deck.Remove(deck[0]);
    }
}
