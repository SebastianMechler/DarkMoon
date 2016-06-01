using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	public float g_MovementSpeed = 4.0f;
    public float g_MovementSpeedNormal = 4.0f;
	public float g_MovementSpeedHaste = 7.0f;

	private GameObject g_LastPatrolSpot;
	private int g_NumberOfActions;
	private int g_CurrentAction;

	private ActionType m_CurrentAction;
	private float m_CurWait;
	private float m_WaitTotal;
	private string m_TargetPatrolName;
    private int m_NumberOfStaticWaypoints;

	// Dynamic Movement Pattern
	public GameObject m_StartDynamicWaypoint;
	public GameObject m_FirstDynamicWaypoint;
	// public while debugging
	private GameObject m_LastWaypoint;
	private GameObject m_NextWaypoint;
    public float m_WaitAfterEachWaypoint = 2.0f;
    private float m_WaitTimer;
	private GameObject[] m_TempWaypointList;
	private GameObject m_NoiseSource;
	private GameObject m_NoiseClosestWaypoint;
	public List<GameObject> finalisedRoute;
	private List<string> finalisedRouteCheck;

	[System.Serializable]
	public struct GroupDistance
	{
		public GameObject m_WaypointFrom;
		public string m_WaypointFrom_Name;
		public GameObject m_WaypointTo;
		public string m_WaypointTo_Name;

		public float m_Distance;
	}
    public GroupDistance[] m_DistanceBetweenGameObjects;

	// Return Movement Speed
	public float getMovementSpeed() { return g_MovementSpeed; }

    private void resetAllData()
    {
        m_MovementPattern = MovementPattern.NONE;
        g_ActionQueue = null;
        g_LastPatrolSpot = null;
        g_NumberOfActions = 0;
        g_CurrentAction = 0;

        m_CurWait = 0.0f;
        m_WaitTotal = 0.01f;
        m_TargetPatrolName = string.Empty;
        m_NumberOfStaticWaypoints = 0;

        m_StartDynamicWaypoint = null;
        m_FirstDynamicWaypoint = null;
        m_TempWaypointList = null;
        m_NoiseSource = null;
        m_NoiseClosestWaypoint = null;

        finalisedRoute = null;
        finalisedRouteCheck = null;

        // m_DistanceBetweenGameObjects = null;
    }

	// public function
	public void changeMovementPattern(MovementPattern replace, GameObject source, GameObject closest)
	{
        // Change from DYNAMIC to STATIC
        if (replace == MovementPattern.STATIC)
	    {
            // Reset Much Data, Much Wow
            resetAllData();

            // A Noise occured, track the source and the closest Waypoint
            m_NoiseSource = source;
            m_NoiseClosestWaypoint = closest;
            // Generate a Path Starting at the Nearest Waypoint to the Enemy,
            //  Leading to the the closest Waypoint of the Noise
            AI_Static_GeneratePath();
            AI_Static_ListToArray();
            // Reset MovementPattern, for the Update Functionality to work differently
            m_MovementPattern = replace;
            // For Noise, we increae the Movement Speed
            g_MovementSpeed = g_MovementSpeedHaste;
            // ..
            return;
        }

        // Change from STATIC to DYNAMIC
	    if (replace == MovementPattern.DYNAMIC)
	    {
            // We take the Current Position 
            g_LastPatrolSpot = gameObject;

            m_LastWaypoint = m_NoiseClosestWaypoint;
            m_NextWaypoint = m_NoiseClosestWaypoint;

            // Patrol Movement
            m_CurrentAction = ActionType.PATROL;
            // Reset MovementPattern, for the Update Functionality to work differently
            m_MovementPattern = replace;
            // For Dynamic Movement, we move at Normal Speed
            g_MovementSpeed = g_MovementSpeedNormal;

            return;
        }
    }

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

		// do crazy stuff
		CalculateAllDistances();
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

                case ActionType.WAITFOR_SECONDS:
			        m_WaitTimer += Time.deltaTime;
			        if (m_WaitTimer >= m_WaitAfterEachWaypoint)
			        {
			            m_CurrentAction = ActionType.PATROL;
			        }
                    break;

				case ActionType.PATROL:
					AI_Dynamic_PatrolMovement();
					break;
			}
		}
	}


    

	void CalculateAllDistances()
	{
		GameObject[] allObjects = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
		GameObject[] childObjects;
		int numberOfObjects = allObjects.Length;

		int sizeCounter = 0;
		for (int i = 0; i < numberOfObjects; i++)
		{
			GameObject current = allObjects[i];
			childObjects = current.GetComponent<WaypointTreeNode>().getAllWaypoints();
			int numberOfChilds = childObjects.Length;
			for (int j = 0; j < numberOfChilds; j++)
			{
				++sizeCounter;
            }
		}
		m_DistanceBetweenGameObjects = new GroupDistance[sizeCounter];

		Vector3 posParent, posChild;
		int indexCounter = 0;

		for (int i = 0; i < numberOfObjects; i++)
		{
			GameObject current = allObjects[i];
			posParent = current.transform.position;

			childObjects = current.GetComponent<WaypointTreeNode>().getAllWaypoints();
			int numberOfChilds = childObjects.Length;
			for(int j = 0; j < numberOfChilds; j++)
			{
				posChild = childObjects[j].transform.position;

				GroupDistance newGroup = new GroupDistance();
				newGroup.m_WaypointFrom = current;
				newGroup.m_WaypointFrom_Name = current.GetComponent<WaypointTreeNode>().getName();
				newGroup.m_WaypointTo = childObjects[j];
				newGroup.m_WaypointTo_Name = childObjects[j].GetComponent<WaypointTreeNode>().getName();
				newGroup.m_Distance = Mathf.Abs(Vector3.Distance(posParent, posChild));

				m_DistanceBetweenGameObjects[indexCounter] = newGroup;
				++indexCounter;
            }
        }


    }

	void OnTriggerEnter(Collider other)
	{
	    // Debug.Log("Colliding gameObject.name: " + other.gameObject.name);
	    if (other.gameObject.name.Equals(StringManager.Names.player) || other.gameObject.name.Equals(StringManager.Resources.debugLvPrototype))
	    {
            return;
	    }

		if (m_MovementPattern == MovementPattern.STATIC)
		{
            string triggerGameObjectName = other.gameObject.GetComponent<WaypointTreeNode>().getName();
            if (other.gameObject.tag == StringManager.Tags.Waypoints && triggerGameObjectName.Equals(m_TargetPatrolName))
			{
				AI_Static_SetNextPatternIndex(other.gameObject);
				
            }
		}

		if (m_MovementPattern == MovementPattern.DYNAMIC)
		{
			string triggerGameObjectName = other.gameObject.GetComponent<WaypointTreeNode>().getName();
			// Debug.Log("Compare '"+triggerGameObjectName+"' (trigger) with '"+m_TargetPatrolName+"' (saved)");
			if (other.gameObject.tag == StringManager.Tags.Waypoints && triggerGameObjectName.Equals(m_TargetPatrolName))
			{
				AI_Dynamic_SetNextWaypoint();
				m_TargetPatrolName = m_NextWaypoint.GetComponent<WaypointTreeNode>().getName();

			    m_WaitTimer = 0.0f;
			    m_CurrentAction = ActionType.WAITFOR_SECONDS;
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
        Vector3 fr = gameObject.transform.position;
        // Vector3 fr = g_LastPatrolSpot.transform.position;
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

        // Debug.Log("[!] Check if Last Waypoint is Reached. ( " + g_CurrentAction + " =?= " + m_NumberOfStaticWaypoints + " )");
        if (g_CurrentAction >= m_NumberOfStaticWaypoints)
        {
            changeMovementPattern(MovementPattern.DYNAMIC, null, null);
            return;
        }

        g_CurrentAction = (g_CurrentAction == g_NumberOfActions ? 0 : g_CurrentAction);
		m_CurrentAction = g_ActionQueue[g_CurrentAction].m_ThisAction;
        m_TargetPatrolName = g_ActionQueue[g_CurrentAction].m_NextPatrolSpot.gameObject.GetComponent<WaypointTreeNode>().getName();
        // Debug.Log("Next Action ID: " + g_CurrentAction);
    }

	void AI_Static_GeneratePath()
	{
	    // Find Nearest Waypoint to current Position
		GameObject[] list = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
		Vector3 thisGameObject = gameObject.transform.position;
		Vector3 next;
		float lastNearestDistance = float.MaxValue;
		GameObject NearestWaypoint = null;
		string nearestWaypointName;

		// Search all Waypoints now
		int size = list.Length;
		for (int i = 0; i < size; i++)
		{
			next = list[i].transform.position;
			float distance = Vector3.Distance(thisGameObject, next);
			if (distance < lastNearestDistance)
			{
				lastNearestDistance = distance;
				NearestWaypoint = list[i];
			}
		}
		nearestWaypointName = NearestWaypoint.GetComponent<WaypointTreeNode>().getName();

		// Find respective Struct Entry for found Waypoint
		int startingIndex = -1;
		int totalNumberOfWaypointConnection = m_DistanceBetweenGameObjects.Length;
		for(int i = 0; i < totalNumberOfWaypointConnection; i++)
		{
			if (nearestWaypointName.Equals(m_DistanceBetweenGameObjects[i].m_WaypointFrom_Name))
			{
				startingIndex = i;
				break;
            }
		}

		int error_Counter = 0;

		// Creating a List for the Static Route the Enemy is meant to follow
		finalisedRoute = new List<GameObject>();
		finalisedRoute.Add(m_DistanceBetweenGameObjects[startingIndex].m_WaypointFrom);
		// Create a Confirmation List for already checked Waypoints
		finalisedRouteCheck = new List<string>();
		finalisedRouteCheck.Add(m_DistanceBetweenGameObjects[startingIndex].m_WaypointFrom_Name);

		GameObject parentToSearch = m_DistanceBetweenGameObjects[startingIndex].m_WaypointFrom;
        GameObject preferredRoute;
		bool routeFound = false;
		while (!routeFound)
		{
			// Go through all childs
			GameObject[] rootChilds = parentToSearch.GetComponent<WaypointTreeNode>().getAllWaypoints();
			int rootChildrenCount = rootChilds.Length;
			float[] distancesToEnemy = new float[rootChildrenCount];
			float[] distancesToNoiseSource = new float[rootChildrenCount];

			// 1. Calculate the Distance between the current Parent and the children each
			// 2. Calculate the Distance between the noise source and the children each
			float checkDistanceEnemy, checkDistanceNoise;
			for (int i = 0; i < rootChildrenCount; i++)
			{
				distancesToEnemy[i] = Mathf.Abs(Vector3.Distance(NearestWaypoint.transform.position, rootChilds[i].transform.position));
				distancesToNoiseSource[i] = Mathf.Abs(Vector3.Distance(m_NoiseSource.transform.position, rootChilds[i].transform.position));
				// 8. If a child is the Waypoint nearest to the Noise Source, we skip the rest
				if (rootChilds[i].GetComponent<WaypointTreeNode>().getName().Equals(m_NoiseClosestWaypoint.GetComponent<WaypointTreeNode>().getName()))
				{
				    // Debug.Log("[!] Closest Waypoint found");
					finalisedRoute.Add(rootChilds[i]);
					routeFound = true;
					break;
				}
			}

			// 9. Skip the rest
			if (routeFound)
			{
				break;
			}

			float curDistanceEnemy = float.MaxValue;
			float curDistanceNoise = 0.0f;
			preferredRoute = rootChilds[0];
			
			for (int i = 0; i < rootChildrenCount; i++)
			{
				checkDistanceEnemy = distancesToEnemy[i];
				checkDistanceNoise = distancesToNoiseSource[i];

				// 3. Let's prepare some float to search for the smalles distances
				float curCheck = curDistanceEnemy + curDistanceNoise;
				// 4. Calculate the Sum of the Distance between Parent and Noise
				float nexCheck = checkDistanceEnemy + checkDistanceNoise;
				
				// 5. Search the smallest Distance to the Noise Source
				if(nexCheck < curCheck)
				{
					// 5.1 Check if Waypoint is already used
					if( !finalisedRouteCheck.Contains(rootChilds[i].GetComponent<WaypointTreeNode>().getName()))
					{
						preferredRoute = rootChilds[i];
						curDistanceEnemy = checkDistanceEnemy;
						curDistanceNoise = checkDistanceNoise;
					}
				}
            }

			// 6. List the smallest Distance as the next Waypoint
			finalisedRoute.Add(preferredRoute);
			// 6.1 Add to known List
			finalisedRouteCheck.Add(preferredRoute.GetComponent<WaypointTreeNode>().getName());
			// 7. Use the closest found Waypoint as the new Parent
			parentToSearch = preferredRoute;

			// Increase, if hit 100 we escape the endless loop
			++error_Counter;
			if(error_Counter >= 100)
			{
				Debug.LogError("[!] Endlosschleife. Abbruch des Pathfindings.");
				routeFound = true;
				break;
			}
        }
    }

    void AI_Static_ListToArray()
    {
        int numberOfElements = finalisedRoute.Count;
        g_ActionQueue = new ActionQueue[numberOfElements];

        m_NumberOfStaticWaypoints = numberOfElements;
        // Debug.Log("[!] finalisedRoute found with '" + numberOfElements + "' Waypoints");
        for (int i = 0; i < numberOfElements; i++)
        {
            Debug.Log(i + ": " + finalisedRoute[i].name);
            g_ActionQueue[i].m_ThisAction = ActionType.PATROL;
            g_ActionQueue[i].m_NextPatrolSpot = finalisedRoute[i];
        }

        m_TargetPatrolName = g_ActionQueue[0].m_NextPatrolSpot.GetComponent<WaypointTreeNode>().getName();
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
        // GameManager.ClearDebugConsole();

        // Debug.Log(" ============= Find Next Waypoint ============= ");
        // Debug.Log("[?] Find all Neighbours for '"+ m_NextWaypoint.gameObject.name + "'");
        m_TempWaypointList = m_NextWaypoint.GetComponent<WaypointTreeNode>().getAllWaypoints();
		int WaypointCount = m_TempWaypointList.Length;
        // Debug.Log("[?] Next Waypoint has '"+WaypointCount+"' Neighbours.");
        bool found = false;
		int RandomNext = -1;
		int tryTillFound = 1;
        while (!found)
		{
			RandomNext = Random.Range(0, WaypointCount);
            // Debug.Log("[:" + tryTillFound + "] Random(0, " + WaypointCount + ") = " + RandomNext);
            string lastName = m_LastWaypoint.GetComponent<WaypointTreeNode>().getName();
            // Debug.Log("[?" + tryTillFound + "] LastName: " + m_LastWaypoint.name);
            string nextName = m_TempWaypointList[RandomNext].GetComponent<WaypointTreeNode>().getName();
            // Debug.Log("[?" + tryTillFound + "] NextName: " + m_TempWaypointList[RandomNext].name);

            // Debug.Log("[:" + tryTillFound + "] Try '" + m_TempWaypointList[RandomNext].name + "' as Next Waypoint. May not be '" + m_LastWaypoint.name + "' though.");
            if (!lastName.Equals(nextName))
			{
                // Debug.Log("[!" + tryTillFound + "] Next Waypoint is set to '" + m_TempWaypointList[RandomNext].name + "' (index: " + RandomNext + ")");
                found = true;
			}
			++tryTillFound;
        }

		m_LastWaypoint = m_NextWaypoint;
		m_NextWaypoint = m_TempWaypointList[RandomNext];
	}
}
