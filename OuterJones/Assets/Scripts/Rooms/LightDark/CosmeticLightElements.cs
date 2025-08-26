using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//i want to have receivers in l3-5 underbelly but they dont really actually do anything :)
public class CosmeticLightElements : MonoBehaviour, RoomUpdateListener
{
    [SerializeField] private RoomsLayout layout;

    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;

    [SerializeField] private GameObject beamOn;
    [SerializeField] private GameObject beamOff;

    [SerializeField] private Room room;
    private BeamModel beam;


    public void Start() {
        this.layout.addRoomUpdateListener(this);
        this.beam = BeamPool.getBeam();
        this.beamOff.SetActive(true);
        this.beamOn.SetActive(false);
    }

    
    public void onRoomUpdate(List<Room> rooms) {
        this.beam.killBeam();
        this.beamOff.SetActive(true);
        this.beamOn.SetActive(false);

        if(this.room.canCastBeam()) {
            this.beam.initBeam(this.transform, this.pointA.position, this.pointB.position, null, null);
            this.beamOff.SetActive(false);
            this.beamOn.SetActive(true);
        }
    }
}
