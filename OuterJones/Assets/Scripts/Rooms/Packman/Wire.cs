using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wire : MonoBehaviour
{
    [SerializeField] private GameObject sparkSprite;

    [SerializeField] private List<Transform> sparkPath;
    
    [SerializeField] private bool directionForward; 

    [SerializeField] private float SPARK_SPEED = 8f;



    public IEnumerator wireAnimation(ButtonManager bm) {
        this.sparkSprite.SetActive(true);
        this.sparkSprite.transform.position = this.sparkPath[0].transform.position;

        for(int i = 1; i < this.sparkPath.Count; i++) {    
            float duration = (this.sparkPath[i].transform.position - this.sparkPath[i-1].transform.position).magnitude / SPARK_SPEED;
            float elapsed = 0f;
            Vector3 start = this.sparkPath[i-1].transform.position;
            Vector3 end = this.sparkPath[i].transform.position;

            while(elapsed < duration) {
                this.sparkSprite.transform.position = Vector3.Lerp(start, end, elapsed/duration);

                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        this.sparkSprite.SetActive(false);
        this.sparkSprite.transform.position = this.sparkPath[0].transform.position;

        bm.onWireFinished();
    }
}
