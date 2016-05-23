using UnityEngine;
using System.Collections;

public class EnemyAiScript : MonoBehaviour {

	public enum MovementPattern
	{
		STATIC,
		DYNAMIC,
		NONE
	}

	public enum ActionType
	{
		NONE,
		PATROL,
		SEARCH,
		WAITFOR_SECONDS,
		WAITFOR_DOOR,
		CHASE,
		RETURN
	}

	public MovementPattern m_MovementPattern;

	// Static Movement Pattern

	[System.Serializable]
	public struct ActionQueue
	{
		public ActionType m_ThisAction;
		public GameObject m_NextPatrolSpot;
		public GameObject m_ConnectedDoor;
		public float m_SearchForSeconds;
		public float m_WaitInSeconds;

		public override string ToString()
		{
			string display = string.Empty;
			switch (m_ThisAction)
			{
				case ActionType.NONE:				display = "Error: No Action defined";																			break;
				case ActionType.PATROL:				display = "Patrol to '"+m_NextPatrolSpot.transform.position+"'";												break;
				case ActionType.SEARCH:				display = "Search nearby Area for '" + m_SearchForSeconds + "' seconds";										break;
				case ActionType.WAITFOR_SECONDS:	display = "Wait for '" + m_SearchForSeconds + "' seconds";														break;
				case ActionType.WAITFOR_DOOR:		display = "Wait for '" + m_ConnectedDoor.name + "' at ("+ m_ConnectedDoor.transform.position+ ") to open";		break;
				case ActionType.CHASE:				display = "Chase the Player";																					break;
				case ActionType.RETURN:				display = "Return to First Action";																				break;
			}
			return display;
		}
	}

	public GameObject g_FieldOfView;
	public ActionQueue[] g_ActionQueue;
	public float g_TurnRate;
	public float g_MovementSpeed;

	private GameObject g_LastPatrolSpot;
	private int g_NumberOfActions;
	private int g_CurrentAction;

	private ActionType m_CurrentAction;
	private float m_CurWait;
	private float m_WaitTotal;
	private string m_TargetPatrolName;

	// Dynamic Movement Pattern
	public GameObject m_StartDynamicWaypoint;
	public GameObject m_FirstDynamicWaypoint;
	// public while debugging
	private GameObject m_LastWaypoint;
	private GameObject m_NextWaypoint;
	private GameObject[] m_TempWaypointList;

	// Use this for initialization
	void Start () {

		if(m_MovementPattern == MovementPattern.STATIC)
		{
			g_NumberOfActions = g_ActionQueue.Length;
			g_LastPatrolSpot = gameObject;
			g_CurrentAction = 0;
			m_CurrentAction = g_ActionQueue[g_CurrentAction].m_ThisAction;
			m_TargetPatrolName = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.gameObject.name;
		}

		if (m_MovementPattern == MovementPattern.DYNAMIC)
		{
			m_LastWaypoint = m_StartDynamicWaypoint;
			m_NextWaypoint = m_FirstDynamicWaypoint;
			m_CurrentAction = ActionType.PATROL;
			m_TargetPatrolName = m_FirstDynamicWaypoint.GetComponent<WaypointTreeNode>().getName();
		}
    }
	
	// Update is called once per frame
	void Update () {

		if ( m_MovementPattern == MovementPattern.STATIC )
		{
			switch (m_CurrentAction)
			{
				case ActionType.NONE:
					Debug.LogError("Not specified Action for Enemy AI");
					break;

				case ActionType.PATROL:
					AI_Static_PatrolMovement();
					break;

				case ActionType.WAITFOR_SECONDS:
					AI_Static_WaitForSeconds();
					break;
			}
		}

		if ( m_MovementPattern == MovementPattern.DYNAMIC)
		{
			// TODO: LateStart or the likes
			if(m_TargetPatrolName.Length <= 1)
			{
				m_TargetPatrolName = m_FirstDynamicWaypoint.GetComponent<WaypointTreeNode>().getName();
			}

			switch (m_CurrentAction)
			{
				case ActionType.NONE:
					Debug.LogError("Not specified Action for Enemy AI");
					break;

				case ActionType.PATROL:
					AI_Dynamic_PatrolMovement();
					break;
			}
		}


	}
	/* ********************************* *
	 *		Static Movement Pattern		 *
	 * ********************************* */
	void AI_Static_WaitForSeconds()
	{
		m_CurWait += Time.deltaTime;
		if(m_CurWait >= g_ActionQueue[g_CurrentAction].m_WaitInSeconds)
		{
			m_CurWait = 0;
            AI_Static_SetNextPatternIndex(g_LastPatrolSpot);
        }
	}

	void AI_Static_PatrolMovement()
	{
		Vector3 to = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.transform.position;
		Vector3 fr = g_LastPatrolSpot.transform.position;
		to.y = 0.0f;
		fr.y = 0.0f;
		Vector3 toTarget = to - fr;
		toTarget.y = 0.0f;

		// Vector3 toTarget = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.transform.position - g_LastPatrolSpot.transform.position;
		float turnRate = g_TurnRate * Time.deltaTime;
		Quaternion lookRotation = Quaternion.LookRotation(toTarget);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnRate);
		transform.Translate(Vector3.forward * g_MovementSpeed * Time.deltaTime);
	}

	bool AI_Static_Setup()
	{
		bool success = false;

		// Define g_ActionQueue here

		return success;
	}

	bool AI_Static_IsOnScene()
	{
		return false;
	}

	void AI_Static_ChangeBehaviour(bool a_IsOnScene)
	{
		// enemy should be a DontDestroyOnLoad 
		if( a_IsOnScene)
		{
			// Enable Gravity
			// Enable Colliders
		}
		else
		{
			// Disable Gravity
			// Disable Colliders
		}
	}

	void AI_Static_SetNextPatternIndex(GameObject a_CurPosition)
	{
		g_LastPatrolSpot = a_CurPosition;
		++g_CurrentAction;
		g_CurrentAction = (g_CurrentAction == g_NumberOfActions ? 0 : g_CurrentAction);
		m_CurrentAction = g_ActionQueue[g_CurrentAction].m_ThisAction;
		// Debug.Log("Next Action ID: " + g_CurrentAction);
	}

	void OnTriggerEnter(Collider other)
	{
		if(m_MovementPattern == MovementPattern.STATIC)
		{
			if (other.gameObject.tag == StringManager.Tags.Waypoints && other.gameObject.name == m_TargetPatrolName)
			{
				AI_Static_SetNextPatternIndex(other.gameObject);
				m_TargetPatrolName = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.gameObject.name;
			}
		}

		if(m_MovementPattern == MovementPattern.DYNAMIC)
		{
			string triggerGameObjectName = other.gameObject.GetComponent<WaypointTreeNode>().getName();
			// Debug.Log("Compare '"+triggerGameObjectName+"' (trigger) with '"+m_TargetPatrolName+"' (saved)");
			if (other.gameObject.tag == StringManager.Tags.Waypoints && triggerGameObjectName.Equals(m_TargetPatrolName))
			{
				AI_Dynamic_SetNextWaypoint();
				m_TargetPatrolName = m_NextWaypoint.GetComponent<WaypointTreeNode>().getName();
            }
		}
		
    }

	/* ********************************* *
	 *		Dynamic Movement Pattern	 *
	 * ********************************* */

	void AI_Dynamic_PatrolMovement()
	{
		// Vector3 from = m_LastWaypoint.transform.position;
		Vector3 from = gameObject.transform.position;
		Vector3 to = m_NextWaypoint.transform.position;
        Vector3 toTarget = to - from;
		toTarget.y = 0.0f;

		// Vector3 toTarget = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.transform.position - g_LastPatrolSpot.transform.position;
		float turnRate = g_TurnRate * Time.deltaTime;
		Quaternion lookRotation = Quaternion.LookRotation(toTarget);
		transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnRate);
		transform.Translate(Vector3.forward * g_MovementSpeed * Time.deltaTime);
	}

	void AI_Dynamic_SetNextWaypoint()
	{
		GameManager.ClearDebugConsole();

		Debug.Log(" ============= Find Next Waypoint ============= ");
		m_TempWaypointList = m_NextWaypoint.GetComponent<WaypointTreeNode>().getAllWaypoints();
		int WaypointCount = m_TempWaypointList.Length;
		Debug.Log("[?] Next Waypoint has '"+WaypointCount+"' Neighbours.");
		bool found = false;
		int RandomNext = -1;
		int tryTillFound = 1;
        while (!found)
		{
			RandomNext = Random.Range(0, WaypointCount);
			Debug.Log("[:" + tryTillFound + "] Random(0, " + WaypointCount + ") = " + RandomNext);
			string lastName = m_LastWaypoint.GetComponent<WaypointTreeNode>().getName();
			Debug.Log("[?" + tryTillFound + "] LastName: " + m_LastWaypoint.name);
			string nextName = m_TempWaypointList[RandomNext].GetComponent<WaypointTreeNode>().getName();
			Debug.Log("[?" + tryTillFound + "] NextName: " + m_TempWaypointList[RandomNext].name);
			
			Debug.Log("[:" + tryTillFound + "] Try '" + m_TempWaypointList[RandomNext].name + "' as Next Waypoint. May not be '" + m_LastWaypoint.name + "' though.");
			if (!lastName.Equals(nextName))
			{
				Debug.Log("[!" + tryTillFound + "] Next Waypoint is set to '" + m_TempWaypointList[RandomNext].name + "' (index: " + RandomNext + ")");
				found = true;
			}
			++tryTillFound;
        }

		m_LastWaypoint = m_NextWaypoint;
		m_NextWaypoint = m_TempWaypointList[RandomNext];
	}
}
