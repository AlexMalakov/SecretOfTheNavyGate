using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternatingSpitterListener : MonoBehaviour
{
    [SerializeField] private GameObject zero;
    [SerializeField] private GameObject one;
    [SerializeField] private GameObject two;
    [SerializeField] private GameObject three;
    [SerializeField] private GameObject four;

    [SerializeField] UnderbellyStaircase targetStaircase;
    [SerializeField] UnderbellyStaircase otherStaircase;

    private Quaternion initialRot;

    int counter = 0;
    bool direction = true;

    public void Awake() {
        this.initialRot = this.transform.rotation;
    }

    public void onSpin(bool spunDirection) {
        if(this.direction != spunDirection) {
            this.direction = spunDirection;
            this.counter = 1;
        } else {
            this.counter++;
        }

        switch(this.counter) {
            case 0:
                disableAll();
                this.four.SetActive(true);
                break;
            case 1:
                disableAll();
                this.three.SetActive(true);
                break;
            case 2:
                disableAll();
                this.two.SetActive(true);
                break;
            case 3:
                disableAll();
                this.one.SetActive(true);
                break;
            case 4:
                disableAll();
                this.zero.SetActive(true);
                this.targetStaircase.onEffect();
                this.otherStaircase.onEffect();
                break;

        }
    }

    private void disableAll() {
        this.zero.SetActive(false);
        this.one.SetActive(false);
        this.two.SetActive(false);
        this.three.SetActive(false);
        this.four.SetActive(false);
    }

    public void rotate90() {
        this.transform.rotation = this.initialRot;
    }
}
