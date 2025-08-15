using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private List<Room> deck;
    private Room currentRoom;
    private bool rotateDirection = true;
    private Inventory inventory;

    [SerializeField] private List<PlayerEdgeCollider> edges = new List<PlayerEdgeCollider>();
    [SerializeField] private List<Transform> mummyTargets;


    public void Start() {
        this.inventory = FindObjectOfType<Inventory>();
        deck = new List<Room>();

        // GameObject obj = GameObject.Find("deckRoom");
        // deck.Add(obj.GetComponent<Room>());

        GameObject obj = GameObject.Find("startingRoom");
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

    public void setRotateDirection(bool rotateDirection) {
        this.rotateDirection = rotateDirection;
    }

    public bool getRotationDirection() {
        return this.rotateDirection;
    }

    public List<Transform> getMummyTargets() {
        return this.mummyTargets;
    }



    public bool hasKey() {
        Debug.Log("HAS KEY?" + (this.inventory.hasItem(PossibleItems.Key) && ((Key)this.inventory.getItem(PossibleItems.Key)).hasKey()));
        return this.inventory.hasItem(PossibleItems.Key) && ((Key)this.inventory.getItem(PossibleItems.Key)).hasKey();
    }

    public void useKey() {
        if(!this.inventory.hasItem(PossibleItems.Key)) {
            Debug.Log("WARNING: NO KEY!");
        }

        ((Key)this.inventory.getItem(PossibleItems.Key)).useKey();
    }

    public Inventory getInventory() {
        return this.inventory;
    }
}
