using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrowthDetector : MonoBehaviour 
{
	Builder[] m_builderList = null;

	private GrowthManager.GrowthStage m_growthStage = GrowthManager.GrowthStage.none;

	private float m_timer = 0f;

	void Start()
	{
		m_builderList = this.GetComponentsInChildren<Builder> ();
	}

	void Update()
	{
		bool isInside = GrowthManager.instance.AmIInsideGrowthPoint (this.gameObject);

		if (isInside) 
		{
			// can grow now
			m_timer += Time.deltaTime;
			if (m_timer > GrowthManager.instance.growthTimes [(int)m_growthStage]) 
			{
				m_timer = 0f;
				m_growthStage++;
			}
		}

		for (int i = 0; i < m_builderList.Length; i++) 
		{
			Builder builder = m_builderList [i];

			builder.grow = isInside && (builder.growthStage == m_growthStage);
		}
	}
}
