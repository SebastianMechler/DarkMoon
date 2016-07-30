using UnityEngine;
using System.Collections;

public class WaypointTreeNode : MonoBehaviour
{
	// private bool m_HasSubWaypoints;
	// private GameObject[] m_SubWayPoints;

	public string m_UniqueName;
    public GameObject[] m_NearbyWaypoints;

	public bool m_StaticDoSearch = true;
	public bool m_FaceNextWaypoint = false;
	public bool m_StaticSlowPatrol = false;

    void Awake()
    {
        m_UniqueName = gameObject.GetHashCode().ToString();
    }

	public GameObject[] getAllWaypoints() { return m_NearbyWaypoints; }
	// public GameObject[] getAllSubWaypoints() { return m_SubWayPoints; }
	// public bool hasSubWaypoints() { return m_HasSubWaypoints; }
	public string getName() { return m_UniqueName; }

	public bool shouldSearchNearby() { return m_StaticDoSearch;  }
	public bool shouldFaceNextWaypoint() { return m_FaceNextWaypoint; }
	public bool shouldSlowWalkPatrolNextWaypoint() { return m_StaticSlowPatrol; }
}
