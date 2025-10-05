using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//only making this cuz i want them to sync up...
public class MapExclamationManager : MonoBehaviour
{
    List<MapExclamation> exclamationImgs = new List<MapExclamation>();
    [SerializeField] private GameObject exclamationPrefab;
    [SerializeField] private int exclamationCount;
    [SerializeField] private float flashTimePause;

    private Dictionary<MapExclamation, Room> flashing = new Dictionary<MapExclamation, Room>();

    public void Awake() {
        for(int i = 0; i < exclamationCount; i++) {
            makeNewExclamation();
        }
    }

    private void makeNewExclamation() {
        this.exclamationImgs.Add(Instantiate(exclamationPrefab, this.transform).GetComponent<MapExclamation>());
        this.exclamationImgs[this.exclamationImgs.Count - 1].setManager(this);
    }

    public void startFlash(Room roomToFlash, RectTransform flashPos, bool countFlash) {
        foreach(MapExclamation excl in this.exclamationImgs) {
            if(!excl.isActive()) {
                this.flashing.Add(excl, roomToFlash);
                excl.startFlash(flashPos, countFlash);
                return;
            }
        }


    }

    public void Start() {
        flashExclamation();
    }

    private void flashExclamation() {
        List<MapExclamation> keyCopy = new List<MapExclamation>(this.flashing.Keys);
        foreach(MapExclamation excl in keyCopy) {
            excl.flashExclamation();
        }

        Invoke(nameof(unflashExclamation), this.flashTimePause);
    }

    private void unflashExclamation() {
        List<MapExclamation> keyCopy = new List<MapExclamation>(this.flashing.Keys);
        foreach(MapExclamation excl in keyCopy) {
            excl.unflashExclamation();
        }

        Invoke(nameof(flashExclamation), this.flashTimePause);
    }

    public void onRoomEntered(Room r) {
        List<MapExclamation> keyCopy = new List<MapExclamation>(this.flashing.Keys);
        foreach(MapExclamation excl in keyCopy) {
            if(this.flashing[excl] == r) {
                excl.endEarly();
                this.onExclamationOver(excl);
            }
        }
    }

    public void onExclamationOver(MapExclamation excl) {
        this.flashing.Remove(excl);
    }
}
