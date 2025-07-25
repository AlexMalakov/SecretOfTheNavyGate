using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RotationPuzzleElement {
    void resetElement();

    void init(Canal c);
    void onPlayerInCanal();
    void onPlayerOutCanal(); 
} 

public class RotationPuzzleManager : MonoBehaviour, Effectable
{
    [SerializeField] private List<GameObject> puzzleElementObjects;

    [SerializeField] private PowerableDoor puzzleEntrance;

    private List<RotationPuzzleElement> puzzleElements;

    public void Awake() {
        foreach(GameObject obj in this.puzzleElementObjects) {
            this.puzzleElements.Add(obj.GetComponent<RotationPuzzleElement>());
        }
    }

    public void resetPuzzle() {
        foreach(RotationPuzzleElement e in this.puzzleElements) {
            e.resetElement();
        }
    }

    public void initElements(Canal c) {
        foreach(RotationPuzzleElement r in this.puzzleElements) {
            r.init(c);
        }
    }

    public void onPlayerInCanal() {
        foreach(RotationPuzzleElement r in this.puzzleElements) {
            r.onPlayerInCanal();
        }
    }

    public void onPlayerOutCanal() {
        foreach(RotationPuzzleElement r in this.puzzleElements) {
            r.onPlayerOutCanal();
        }
    }

    public void onEffect() {
        this.resetPuzzle();
        puzzleEntrance.opencloseDoor(true);
    }

    void OnTriggerEnter2D(Collider2D other) {
        if(other.GetComponent<Player>() != null) {
            puzzleEntrance.opencloseDoor(false);
        }
    }
}
