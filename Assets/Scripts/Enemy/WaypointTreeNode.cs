using UnityEngine;
using System.Collections;

public class WaypointTreeNode : MonoBehaviour
{
    public bool m_HasSubWaypoints;
    public GameObject[] m_SubWayPoints;

    public string m_UniqueName;
    public GameObject[] m_NearbyWaypoints;

    void Start()
    {
        m_UniqueName = gameObject.GetHashCode().ToString();
    }

	public GameObject[] getAllWaypoints() { return m_NearbyWaypoints; }
	public GameObject[] getAllSubWaypoints() { return m_SubWayPoints; }
	public bool hasSubWaypoints() { return m_HasSubWaypoints; }
	public string getName() { return m_UniqueName; }
}
