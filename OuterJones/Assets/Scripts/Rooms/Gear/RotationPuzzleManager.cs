using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface RotationPuzzleElement {
    void resetElement();
} 

public class RotationPuzzleManager : MonoBehaviour
{
    [SerializeField] private List<RotationPuzzleElement> puzzleElements;

    public void resetPuzzle() {
        foreach(RotationPuzzleElement e in this.puzzleElements) {
            e.resetElement();
        }
    }
}
