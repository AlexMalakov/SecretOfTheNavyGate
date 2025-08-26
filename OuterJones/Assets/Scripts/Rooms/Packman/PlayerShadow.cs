using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private List<Transform> mummyTargets;
    [SerializeField] private Player player;
    private PackmanRoom chaseIn;
    private bool chasing = false;

    public void endTask() {
        this.chasing = false;
    }

    public void wakeShadow(PackmanRoom pRoom) {
        this.chaseIn = pRoom;
        this.chasing = true;
    }

    public List<Transform> getMummyTargets() {
        return this.mummyTargets;
    }

    void Update() {
        if(chasing) {
            this.transform.position = this.chaseIn.transform.position + player.transform.position - player.getCurrentRoom().transform.position;
        }
    }
}
