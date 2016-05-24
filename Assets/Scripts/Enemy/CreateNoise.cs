using UnityEngine;
using System.Collections;

public class CreateNoise : MonoBehaviour {

	public GameObject m_Enemy;
	public string m_UniqueName;
	public bool m_PlaySoundOnEnter;
	public GameObject m_NearestWaypoint;
	float lastNearestDistance = float.MaxValue;

	// Use this for initialization
	void Awake ()
	{
		m_UniqueName = gameObject.GetHashCode().ToString();

		GameObject[] list = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
		Vector3 thisGameObject = gameObject.transform.position;
		Vector3 next;

		int size = list.Length;
		for (int i = 0; i < size; i++)
		{
			next = list[i].transform.position;
			float distance = Vector3.Distance(thisGameObject, next);
			if (distance < lastNearestDistance)
			{
				lastNearestDistance = distance;
				m_NearestWaypoint = list[i];
			}
		}
		// Debug.Log("[!] Shortest Distance starting from " + m_UniqueName + ": " + m_NearestWaypoint.name);
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == StringManager.Tags.player)
		{
			if (m_PlaySoundOnEnter)
			{
				gameObject.GetComponent<AudioSource>().Play();
			}

			m_Enemy.GetComponent<EnemyAiScript>().changeMovementPattern(EnemyAiScript.MovementPattern.NONE, gameObject, m_NearestWaypoint);
        }
	}

	public string getName()
	{
		return m_UniqueName;
	}

	public GameObject getNearestWaypoint()
	{
		return m_NearestWaypoint;
	}
}
