using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrowthManager : MonoBehaviour 
{
	private static GrowthManager m_instance = null;

	public static GrowthManager instance { get { return m_instance; } }

	private List<GrowthPoint> m_growths = new List<GrowthPoint> ();

	public enum GrowthStage
	{
		none,
		vines,
		shrubs,
		flowers,
		bees
	};

	public float[] growthTimes;

	void Awake()
	{
		if (m_instance != null) 
		{
			Destroy (this.gameObject);
		}

		m_instance = this;
	}

	public void AddGrowthPoint(GrowthPoint growth)
	{
		m_growths.Add (growth);
	}

	public void RemoveGrowthPoint(GrowthPoint growth)
	{
		m_growths.Remove (growth);
	}

	public bool AmIInsideGrowthPoint(GrowthDetector detector)
	{
		for (int i = 0; i < m_growths.Count; i++) 
		{
			GrowthPoint growth = m_growths [i];
			Vector3 vec = detector.transform.position - growth.transform.position;

			if (vec.magnitude < growth.radius)
				return true;
		}
		return false;
	}
}
