using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Room> deck;
    private Room currentRoom;

    [SerializeField] List<PlayerEdgeCollider> edges = new List<PlayerEdgeCollider>();


    public void Start() {
        deck = new List<Room>();

        GameObject obj = GameObject.Find("deckRoom");
        deck.Add(obj.GetComponent<Room>());

        obj = GameObject.Find("startingRoom");
        this.currentRoom = obj.GetComponent<Room>();

        FindObjectOfType<DeckUI>().init(this);
    }

    public void addToDeck(List<Room> newDeck) {
        this.deck.AddRange(newDeck);
    }

    public Room getNextInDeck() {
        if(deck.Count == 0)
            return null;
        return deck[0];
    }

    public int getDeckSize() {
        return this.deck.Count;
    }

    public Room getCurrentRoom() {
        return this.currentRoom;
    }

    public void setCurrentRoom(Room r) {
        this.currentRoom = r;
    }

    //throws out of bounds exception, should never occur tho so it going unhandled is best
    public void removeNextInDeck() {
        deck.Remove(deck[0]);
    }

    public List<PlayerEdgeCollider> getEdgeColliders() {
        return this.edges;
    }
}
