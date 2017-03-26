using UnityEngine;

// Simple virtual trackball script ripped off the internet, modified for (even more) simplicity
// + added Zoom in/out
public class VirtualTrackballCamera : MonoBehaviour
{
	[SerializeField]
	private float m_distance = 5f;

	[SerializeField]
	private float m_virtualTrackballDistance = 0.25f;

	[SerializeField]
	private GameObject m_target;

	[SerializeField]
	private Vector2 m_zoomDistMinMax = new Vector2(5f, 20f);

	private Vector3? lastMousePosition;

	// Use this for initialization
	void Start ()
	{
		Vector3 startPos = (this.transform.position - m_target.transform.position).normalized * m_distance;
		Vector3 position = startPos + m_target.transform.position;

		transform.position = position;
		transform.LookAt(m_target.transform.position);
	}

	// Update is called once per frame
	void LateUpdate ()
	{
		Vector3 mousePos = Input.mousePosition;

		bool mouseBtn = Input.GetMouseButton (0);
		if (mouseBtn && (mousePos.y > Screen.height / 4))
		{
			if (lastMousePosition.HasValue) 
			{
				// we are moving from here
				Vector3 lastPos = this.transform.position;
				Vector3 targetPos = m_target.transform.position;

				Quaternion rotation = FigureOutAxisAngleRotation(lastMousePosition.Value, mousePos);

				Vector3 vecPos = (targetPos - lastPos).normalized * -m_distance;

				this.transform.position = rotation * vecPos + targetPos;
				this.transform.LookAt(targetPos);

				lastMousePosition = mousePos;
			} 
			else 
			{
				lastMousePosition = mousePos;
			}
		} 
		else 
		{
			lastMousePosition = null;
		}

		if((Input.mouseScrollDelta.y > 0f) || (Input.mouseScrollDelta.y < 0f))
		{
			Zoom(Input.mouseScrollDelta.y / 10f);
		}
	}

	void Zoom(float normAmount)
	{
		float addDistance = (Input.mouseScrollDelta.y / 10f) * 5f; 
		m_distance -= addDistance;
		m_distance = Mathf.Clamp(m_distance, m_zoomDistMinMax.x, m_zoomDistMinMax.y);
		Vector3 startPos = (this.transform.position - m_target.transform.position).normalized * m_distance;
		Vector3 position = startPos + m_target.transform.position;

		transform.position = position;
	}

	Quaternion FigureOutAxisAngleRotation (Vector3 lastMousePos, Vector3 mousePos)
	{
		if(Mathf.Approximately(lastMousePos.x, mousePos.x) && Mathf.Approximately(lastMousePos.y, mousePos.y))
			return Quaternion.identity;

		Vector3 near = new Vector3(0,0,Camera.main.nearClipPlane);

		Vector3 p1 = Camera.main.ScreenToWorldPoint( lastMousePos + near );
		Vector3 p2 = Camera.main.ScreenToWorldPoint( mousePos + near);

		Vector3 axisOfRotation = Vector3.Cross(p2, p1);

		float twist = (p2 - p1).magnitude / (2.0f * m_virtualTrackballDistance);
		twist = Mathf.Clamp(twist, -1f, 1f);

		float angle = (2.0f * Mathf.Asin(twist)) * 180/Mathf.PI ;

		return Quaternion.AngleAxis(angle, axisOfRotation);
	}
}
