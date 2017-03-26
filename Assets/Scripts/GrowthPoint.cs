using UnityEngine;
using System.Collections;

public class GrowthPoint : MonoBehaviour 
{
	public float speed;

	private float m_radius = 0.0f;

	public float radius { get { return m_radius; } }
	void Start()
	{
		GrowthManager.instance.AddGrowthPoint (this);
	}
	void OnDestroy()
	{
		GrowthManager.instance.RemoveGrowthPoint (this);
	}

	void Update()
	{
		m_radius += speed;
	}

	void OnDrawGizmos()
	{
		Gizmos.DrawWireSphere (this.transform.position, m_radius);
	}
}
