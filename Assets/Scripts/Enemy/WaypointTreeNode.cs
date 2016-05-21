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
}
