using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathfindingAlgorithm : MonoBehaviour {
    
    [System.Serializable]
    public struct GroupDistance
    {
        public Vector3 m_WaypointFrom;
        public Vector3 m_WaypointTo;
        public int m_Index;
        public float m_Distance;
    }

    public struct PosCheck
    {
        public Vector3 a;
        public Vector3 b;
    }
    
    public List<GroupDistance> m_Distances;
    
    void Start()
    {
        m_Distances = CalcAllDistances();
    }

    /// <summary>
    /// Minor functionality to assure the List is always with one Specific Vector in "from" and the other in "to" for List<GroupDistance>
    /// </summary>
    /// <param name="posA">Any Vector3</param>
    /// <param name="posB">Any other Vector3</param>
    /// <returns>result with a being higher in X, Y and Z, b being the other</returns>
    private PosCheck checkVector3(Vector3 posA, Vector3 posB)
    {
        PosCheck result;
        // Set PosB is further than PosA
        result.a = posB;
        result.b = posA;
        // Check if PosA is further than PosB
        if (posA.x > posB.x && posA.y > posB.y && posA.z > posB.z)
        {
            result.a = posA;
            result.b = posB;
        }

        return result;
    }

    private List<GroupDistance> CalcAllDistances()
    {
        List<GroupDistance> distList = new List<GroupDistance>();
        int indexCounter = 0;

        List<Vector3> posList = new List<Vector3>();
        GameObject[] objectArray = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
        int waypointCount = objectArray.Length;
        for (int i = 0; i < waypointCount; i++)
        {
            posList.Add(objectArray[i].transform.position);
        }
        
        for (int i = 0; i < waypointCount; i++)
        {
            GameObject[] childs = objectArray[i].GetComponent<WaypointTreeNode>().getAllWaypoints();
            int childCount = childs.Length;

            for (int j = 0; j < childCount; j++)
            {
                PosCheck check = checkVector3(objectArray[i].transform.position, childs[j].transform.position);

                GroupDistance element;
                element.m_WaypointFrom = check.a;
                element.m_WaypointTo = check.b;
                element.m_Index = indexCounter;
                element.m_Distance = Mathf.Abs(Vector3.Distance(element.m_WaypointFrom, element.m_WaypointTo));

                if (!distList.Contains(element))
                {
                    distList.Add(element);
                }

                ++indexCounter;     // no need to fullfill auto_increment, unique only
            }
        }

        for (int i = 0; i < waypointCount; i++)
        {
            Vector3 curPosition = objectArray[i].transform.position;
            for (int j = 0; j < waypointCount; j++)
            {
                if (i != j)
                {
                    Vector3 anyNext = objectArray[j].transform.position;

                    PosCheck check = checkVector3(curPosition, anyNext);

                    GroupDistance element;
                    element.m_WaypointFrom = check.a;
                    element.m_WaypointTo = check.b;
                    element.m_Index = indexCounter;
                    element.m_Distance = Mathf.Abs(Vector3.Distance(element.m_WaypointFrom, element.m_WaypointTo));

                    if (!distList.Contains(element))
                    {
                        distList.Add(element);
                    }

                    ++indexCounter;     // no need to fullfill auto_increment, unique only
                }
            }
        }

        objectArray = null;
        return distList;
    }

}
