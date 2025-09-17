using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoLightBeams : MonoBehaviour, RoomUpdateListener, Effectable
{
    [SerializeField] private RoomsLayout layout;

    [SerializeField] private List<Transform> fakeBeamPath;

    [SerializeField] private GameObject beamOn;
    [SerializeField] private GameObject beamOff;

    [SerializeField] private Room room;

    [SerializeField] private GameObject effectableObj;
    private List<BeamModel> beams = new List<BeamModel>();
    private bool activated = false;


    public void Awake() {
        this.layout.addRoomUpdateListener(this);

        for(int i = 1; i < this.fakeBeamPath.Count; i++) {
            this.beams.Add(BeamPool.getBeam());
            this.beams[i-1].claimBeam(this.transform);
        }
        
        this.beamOff.SetActive(true);
        this.beamOn.SetActive(false);
    }

    public void onEffect() {
        this.activated = true;
        if(this.room.canCastBeam()) {
            this.beamOff.SetActive(false);
            this.beamOn.SetActive(true);
            for(int i = 0; i < this.beams.Count; i++) {
                this.beams[i].initBeam(this.transform, this.fakeBeamPath[i].position, this.fakeBeamPath[i+1].position, null, null);
            }

            this.effectableObj.GetComponent<Effectable>().onEffect();
        } else {
            foreach(BeamModel beam in this.beams) {
                beam.killBeam();
                beam.claimBeam(this.transform);
                this.beamOff.SetActive(true);
                this.beamOn.SetActive(false);
            }

            this.effectableObj.GetComponent<Effectable>().onEffectOver();
        }
    }

    public void onEffectOver(){}

    
    public void onRoomUpdate(List<Room> rooms) {
        if(this.activated) {
            this.onEffect();
        }
    }
}
