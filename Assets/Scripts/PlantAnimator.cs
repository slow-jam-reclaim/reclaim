using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantAnimator : MonoBehaviour {

    public VineGenerator generator;
    public float timescale;

    float time = 0.0f;

	// Update is called once per frame
	void Update () {
        bool isInside = GrowthManager.instance.AmIInsideGrowthPoint(this.gameObject);
        if(isInside) {
            time += Time.deltaTime * timescale;
        }
        updateCylindersRecursive(generator.root_segment);
	}

    void updateCylindersRecursive(SegmentDataVine segment) {
        Vector3 segmentScale = segment.visual.transform.localScale;
        Vector3 segmentPosition = segment.start;
        if(time < segment.timeStart) {
            segmentScale.y = 0;
        }
        else if(time > segment.timeEnd) {
            segmentScale.y = segment.length / 2; //Cylinders have length 2
            segmentPosition += segment.dir * segment.length / 2;
        }
        else {
            float scalar = (time - segment.timeStart) / (segment.timeEnd - segment.timeStart);
            scalar = -1.0f/2.0f * (Mathf.Cos(Mathf.PI*scalar) - 1.0f);
            segmentScale.y = scalar * segment.length / 2;
            segmentPosition += segment.dir * segment.length * scalar * 0.5f;
        }

        segment.visual.transform.localScale = segmentScale;
        segment.visual.transform.position = segmentPosition;

        if(segmentScale.y == 0) {
            segment.visual.gameObject.SetActive(false);
        }
        else {
            segment.visual.gameObject.SetActive(true);   
        }

        foreach(SegmentDataVine child in segment.children) {
            updateCylindersRecursive(child);
        }
    }
}
