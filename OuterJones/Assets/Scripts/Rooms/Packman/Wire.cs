using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private GameObject sparkSprite;
    [SerializeField] private List<Transform> sparkPath;

    private float SPARK_SPEED = 2f;

    [SerializeField] private PowerableObject nextToPower;


    public IEnumerator followPath() {
        this.sparkSprite.transform.position = this.sparkPath[0].transform.position;
        for(int i = 1; i < this.sparkPath.Count; i++) {
            
            float duration = (this.sparkPath[i].transform.position - this.sparkPath[i-1].transform.position).magnitude / SPARK_SPEED;
            float elapsed = 0f;
            while(elapsed < duration) {
                this.sparkSprite.transform.position = Vector3.Lerp(this.sparkPath[i-1].transform.position, this.sparkPath[i].transform.position, elapsed/duration);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        nextToPower.onPowered();
    }
}
