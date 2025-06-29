using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour, PowerableObject
{
    [SerializeField] private GameObject sparkSprite;
    [SerializeField] private GameObject smokeSprite;

    [SerializeField] private List<Transform> sparkPath;

    private float SPARK_SPEED = 2f;

    [SerializeField] private PowerableObject nextToPower;

    private bool resetting = false;

    public void onPowered() {
        StartCoroutine(followPath());
    }

    public IEnumerator followPath() {
        this.sparkSprite.SetActive(true);
        this.sparkSprite.transform.position = this.sparkPath[0].transform.position;
        for(int i = 1; i < this.sparkPath.Count; i++) {
            
            float duration = (this.sparkPath[i].transform.position - this.sparkPath[i-1].transform.position).magnitude / SPARK_SPEED;
            float elapsed = 0f;
            while(elapsed < duration) {
                if(this.resetting) {
                    this.sparkSprite.SetActive(false);
                    this.smokeSprite.SetActive(true);
                    this.smokeSprite.transform.position = this.sparkSprite.transform.position;
                    this.sparkSprite.transform.position = this.sparkPath[0].transform.position;
                    this.resetting = false;
                    Invoke(nameof(removeSmoke), 1f);
                    yield break;
                }

                this.sparkSprite.transform.position = Vector3.Lerp(this.sparkPath[i-1].transform.position, this.sparkPath[i].transform.position, elapsed/duration);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        this.sparkSprite.SetActive(false);
        this.sparkSprite.transform.position = this.sparkPath[0].transform.position;
        nextToPower.onPowered();
    }

    public void reset() {
        if(sparkSprite.transform.position.x != sparkPath[0].transform.position.x
            || sparkSprite.transform.position.y != sparkPath[0].transform.position.y)
        resetting = true;
    }

    private void removeSmoke() {
        this.smokeSprite.SetActive(false);
    }
}
