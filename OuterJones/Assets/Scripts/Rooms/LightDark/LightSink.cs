using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSink : MonoBehaviour
{
    private bool beamed = false;

    public void activate() {
        this.beamed = true;
    }

    public void deactivate() {
        this.beamed = false;
    }

    public bool getActive() {
        return this.beamed;
    }
}
