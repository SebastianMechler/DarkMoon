using UnityEngine;
using System.Collections;

public class WaypointSystem : MonoBehaviour
{

    enum EnemyState
    {
        NONE,       // State
        IDLE,       // Test State, Pause Enemy
        PATROL,     // Patrol all Waypoints
        CHASE,      // Give Chase to the Player
        RETURN      // Lost Player, Back to Patrol
    }

    [System.Serializable]
    public struct WayPoints
    {
        public GameObject m_ThisLocation;   // May be Lost while Iterating
        public GameObject m_NextLocation;
    }

    public WayPoints[] m_WayPointLocations = null;
    public float m_WalkingSpeed = 5F;

    private int m_WaypointCount = 0;
    private int m_CurrentPatrol = 0;
    private EnemyState m_EnemyState = EnemyState.NONE;
    private Vector3 m_Direction = Vector3.zero;

    private GameObject m_LastCollidedWaypoint = null;

	// Use this for initialization
	void Start ()
	{
        m_WaypointCount = m_WayPointLocations.Length;
	    m_CurrentPatrol = 0;
	    m_EnemyState = EnemyState.PATROL;

	    if (!CheckForWaypoints())
	    {
	        m_EnemyState = EnemyState.NONE;
            GetComponent<WaypointSystem>().enabled = false;
            return;
	    }

	    m_Direction = CalculateNextDirection(0);
	    transform.position = m_WayPointLocations[0].m_ThisLocation.transform.position;
	}

    bool CheckForWaypoints()
    {
        return (m_WaypointCount > 1);
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.F1))
        {
            if (m_EnemyState == EnemyState.IDLE)
            {
                m_EnemyState = EnemyState.PATROL;
            }
            else
            {
                m_EnemyState = EnemyState.IDLE;
            }
        }

    }
	
    bool PlayerDetectedNearby()
    {
        return false;
    }

    
    bool NearbyAccusticSignalDetected()
    {
        return false;
    }

    Vector3 CalculateNextDirection(int a_CurrentPatrolIndex)
    {
        Vector3 result = Vector3.zero;

        int iNext = (a_CurrentPatrolIndex == m_WaypointCount ? 0 : a_CurrentPatrolIndex++);

        result = m_WayPointLocations[a_CurrentPatrolIndex].m_NextLocation.transform.position -
                 m_WayPointLocations[a_CurrentPatrolIndex].m_ThisLocation.transform.position;

        Debug.DrawRay(transform.position, result, Color.red);

        return result.normalized;
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == StringManager.Tags.Waypoints)
        {
            m_LastCollidedWaypoint = col.gameObject;
            m_LastCollidedWaypoint.SetActive(false);
            // Calculate Direction between Current Waypoint and Next Waypoint
            m_Direction = CalculateNextDirection(m_CurrentPatrol);
            // Rotate towards next Location
            // transform.rotation = Quaternion.LookRotation(m_Direction);
            // transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(m_Direction), 1);
            // Prepare for next Waypoint
            m_CurrentPatrol = (m_CurrentPatrol == m_WaypointCount ? 0 : m_CurrentPatrol++);
        }
    }

    void OnTriggerLeave(Collider col)
    {
        if (col.gameObject == m_LastCollidedWaypoint)
        {
            m_LastCollidedWaypoint.SetActive(true);
            m_LastCollidedWaypoint = null;
        }
    }

    void FixedUpdate()
    {
        if (m_EnemyState == EnemyState.PATROL)
        {
            transform.Translate(m_Direction * m_WalkingSpeed * Time.deltaTime);
        }
    }
}
