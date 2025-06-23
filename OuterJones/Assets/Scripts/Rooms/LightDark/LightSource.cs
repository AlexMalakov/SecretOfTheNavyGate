using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    [SerializeField] private Room originRoom;
    [SerializeField] private RoomsLayout layout;

    private DoorDirection castDirection = DoorDirection.North;

    public void castBeam() {

    }
}
