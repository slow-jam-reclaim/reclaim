using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineGenerator : MonoBehaviour {

    public SegmentDataVine root_segment;

    public float vineSpreadRadiusMid;
    public float vineSpreadRadiusRange;
    float vineSpreadRadius;
    public float minSegmentLength;
    public float maxSegmentLengthMid;
    public float maxSegmentLengthRange;
    float maxSegmentLength;

    public float dragStrength;

    public float[] branchChances;

    public float segmentXZScale;
    public float scaleFallofPower;

    public Vector3 goalOffset;
    public GameObject segmentPrefab;
    Vector3 goalCenter;

	// Use this for initialization
	void Start () {

        vineSpreadRadius = Random.Range(vineSpreadRadiusMid - vineSpreadRadiusRange / 2 , vineSpreadRadiusMid + vineSpreadRadiusRange / 2);
        maxSegmentLength = Random.Range(maxSegmentLengthMid - maxSegmentLengthMid / 2, maxSegmentLengthMid + maxSegmentLengthMid / 2);

        SegmentDataVine seed_segment = new SegmentDataVine();
        seed_segment.start = transform.position;
        seed_segment.dir = new Vector3(0, 1, 0);
        seed_segment.length = 0.0f;
        seed_segment.depth = -1;
        seed_segment.timeStart = 0;
        seed_segment.timeEnd = 0;

        goalCenter = transform.position;
        goalCenter.y = 0;

        goalCenter += transform.rotation * goalOffset;
        root_segment = generateChildSegment(seed_segment);

        generateChildrenRecursive(root_segment);

        drawCylindersRecursive(root_segment);
        
	}

    void drawCylindersRecursive(SegmentDataVine _parent) {
        _parent.visual = GameObject.Instantiate(segmentPrefab);
        _parent.visual.transform.rotation = Quaternion.FromToRotation(Vector3.up, _parent.dir);

        _parent.visual.transform.position = _parent.start + _parent.dir * (_parent.length / 2);
        float xz_scale = 1.0f/(Mathf.Pow(_parent.depth + 1, scaleFallofPower));
        _parent.visual.transform.localScale = new Vector3(segmentXZScale * xz_scale, _parent.length / 2, segmentXZScale * xz_scale);

        float h,s,v;
        Color.RGBToHSV(_parent.visual.GetComponent<Renderer>().material.color, out h, out s, out v);
        s += Random.Range(-0.5f, 0.0f);
        _parent.visual.GetComponent<Renderer>().material.color = Color.HSVToRGB(h, s, v);
        foreach(SegmentDataVine child in _parent.children) {
            drawCylindersRecursive(child);
        }
    }

    void generateChildrenRecursive(SegmentDataVine _parent) {
        if(_parent.depth > 5) return;

        float childNumberRoll = Random.value;
        float probSum = 0;
        int childrenCount = 0;
        for(int i=0; i< branchChances.Length; i++) {
            probSum += branchChances[i];
            if(childNumberRoll <= probSum) {
                childrenCount = i;
                break;
            }
        }
        for(int i = 0; i<childrenCount; i++) {
            SegmentDataVine child = generateChildSegment(_parent);
            _parent.children.Add(child);
            generateChildrenRecursive(child);
        }
    }

    SegmentDataVine generateChildSegment(SegmentDataVine _parent) {
        Vector3 child_dir = Random.insideUnitCircle * vineSpreadRadius;
        child_dir.z = child_dir.y;
        float segment_length = Random.Range(minSegmentLength, maxSegmentLength);
        child_dir.y = segment_length;

        Vector3 segment_start = _parent.start + _parent.dir * _parent.length;

        Vector3 drag = child_dir + segment_start;
        drag.y = 0;
        drag = goalCenter - drag;
        drag *= dragStrength;

        child_dir += drag;

        SegmentDataVine child_segment = new SegmentDataVine();
        child_segment.start = segment_start;
        child_segment.dir = child_dir.normalized;
        child_segment.length = child_dir.magnitude;
        child_segment.children = new List<SegmentDataVine>();
        child_segment.depth = _parent.depth + 1;
        child_segment.timeStart = _parent.timeEnd;
        child_segment.timeEnd = child_segment.timeStart + child_segment.length;
        return child_segment;
    }
}

public class SegmentDataVine {
    public Vector3 start;
    public Vector3 dir;
    public float length;
    public int depth;
    public List<SegmentDataVine> children;

    public float timeStart;
    public float timeEnd;

    public GameObject visual;
}