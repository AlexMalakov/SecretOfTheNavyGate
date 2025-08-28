using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirrors : MonoBehaviour
{
    [SerializeField] List<Mirror> mirrors;

    [SerializeField] private DoorDirection northReflectsTo;


    public Transform getHorizontalPoint() {
        return this.mirrors[0].transform;
    }

    public Transform getVerticalPoint() {
        return this.mirrors[mirrors.Count - 1].transform;
    }

    public void displayBeams() {

    }
}
