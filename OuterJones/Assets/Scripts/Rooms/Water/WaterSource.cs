using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSource : MonoBehaviour
{
    [SerializeField] RoomsLayout layout;
    [SerializeField] Canal waterOrigin;

    public void onWaterUpdate() {
        this.drainAll();
        this.computeFlow();
    }

    private void drainAll() {
        foreach(Room r in this.layout.getAllRooms()) {
            r.drainWater();
        }
    }

    private void computeFlow() {
        waterOrigin.onFlood(new List<CanalEntrances>());
    }
}
