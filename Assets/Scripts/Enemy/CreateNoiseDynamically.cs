using UnityEngine;
using System.Collections;

public class CreateNoiseDynamically : MonoBehaviour
{

    public GameObject m_Enemy;
    public string m_UniqueName;
    //public bool m_PlaySoundOnEnter;
    private GameObject m_NearestWaypoint;

    float lastNearestDistance = float.MaxValue;

    public string getName()
    {
        return m_UniqueName;
    }

    public GameObject getNearestWaypoint()
    {
        return m_NearestWaypoint;
    }

    public void UpdateNearestWaypoint()
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

        Debug.Log("Nearest waypoint dynamically set to: " + m_NearestWaypoint.gameObject.name);
    }

    public void MakeNoiseAtCurrentPosition()
    {
        UpdateNearestWaypoint();
        m_Enemy.GetComponent<EnemyAiScript>().changeMovementPattern(EnemyAiScript.MovementPattern.STATIC, gameObject, m_NearestWaypoint);
    }

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag.Equals(StringManager.Tags.player))
        {
            MakeNoiseAtCurrentPosition();
        }

    }
}
