using UnityEngine;
using System.Collections;

public class Builder : MonoBehaviour
{
	//public Material material;

	public float minY = 0;
	public float maxY = 100;
	public float duration = 5;

	public GrowthManager.GrowthStage growthStage = GrowthManager.GrowthStage.vines;

	Material m_material;
	float m_time = 0f;

	public bool grow = false;

	void Awake()
	{
		MeshRenderer renderer = this.GetComponent<MeshRenderer> ();
		m_material = renderer.material;
	}

	void Update () 
	{
		if (grow) 
		{
			m_time += Time.deltaTime;
			float y = Mathf.Lerp (minY, maxY, m_time / duration);
			m_material.SetFloat ("_ConstructY", y);
		}
	}
}
