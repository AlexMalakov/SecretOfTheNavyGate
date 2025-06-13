using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Dam : MonoBehaviour
{
    [SerializeField] private Canal c1;
    [SerializeField] private Canal c2;

    [SerializeField] private bool open;



    public void onFlood(Canal c, List<FloodEntrances> floodingFrom) {
        if(!open) {
            return;
        }

        if(c == c1) {
            c2.onFlood(floodingFrom)
        }else if(c == c2) {
            c1.onFlood(floodingFrom)
        }
    }
}
