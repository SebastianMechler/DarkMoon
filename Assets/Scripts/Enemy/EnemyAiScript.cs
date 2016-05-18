using UnityEngine;
using System.Collections;

public class EnemyAiScript : MonoBehaviour {

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

	// Use this for initialization
	void Start () {
		g_NumberOfActions = g_ActionQueue.Length;
		g_LastPatrolSpot = gameObject;
		g_CurrentAction = 0;
		m_CurrentAction = g_ActionQueue[g_CurrentAction].m_ThisAction;
		m_TargetPatrolName = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.gameObject.name;
    }
	
	// Update is called once per frame
	void Update () {
		switch (m_CurrentAction)
		{
			case ActionType.NONE:
				Debug.LogError("Not specified Action for Enemy AI");
				break;

			case ActionType.PATROL:
				AI_PatrolMovement();
				break;

			case ActionType.WAITFOR_SECONDS:
				AI_WaitForSeconds();
                break;
		}

		
	}

	void AI_WaitForSeconds()
	{
		m_CurWait += Time.deltaTime;
		if(m_CurWait >= g_ActionQueue[g_CurrentAction].m_WaitInSeconds)
		{
			m_CurWait = 0;
            AI_SetNextPatternIndex(g_LastPatrolSpot);
        }
	}

	void AI_PatrolMovement()
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

	bool AI_Setup()
	{
		bool success = false;

		// Define g_ActionQueue here

		return success;
	}

	bool AI_IsOnScene()
	{
		return false;
	}

	void AI_ChangeBehaviour(bool a_IsOnScene)
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

	void AI_SetNextPatternIndex(GameObject a_CurPosition)
	{
		g_LastPatrolSpot = a_CurPosition;
		++g_CurrentAction;
		g_CurrentAction = (g_CurrentAction == g_NumberOfActions ? 0 : g_CurrentAction);
		m_CurrentAction = g_ActionQueue[g_CurrentAction].m_ThisAction;
		// Debug.Log("Next Action ID: " + g_CurrentAction);
	}

	void OnTriggerEnter(Collider other)
	{
		if (other.gameObject.tag == StringManager.Tags.Waypoints && other.gameObject.name == m_TargetPatrolName)
		{
			AI_SetNextPatternIndex(other.gameObject);
			m_TargetPatrolName = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.gameObject.name;
		}
    }
}
