using UnityEngine;
using System.Collections;

public class CreateNoiseDynamically : MonoBehaviour
{

    private GameObject m_Enemy;
    public string m_UniqueName;
    //public bool m_PlaySoundOnEnter;
    public bool m_GenerateNoiseOnGroundContact = false;
    private GameObject m_NearestWaypoint;

    float lastNearestDistance = float.MaxValue;

    void Start()
    {
        m_Enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);
    }

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
        Debug.Log("Get Enemy to target '"+ m_NearestWaypoint.name +"' as a noise source");
        m_Enemy.GetComponent<EnemyAiScript>().changeMovementPattern(EnemyAiScript.MovementPattern.STATIC, gameObject, m_NearestWaypoint);
        gameObject.GetComponent<CreateNoiseDynamically>().enabled = false;
    }

    public void OnCollisionEnter(Collision other)
    {
        Debug.Log("Thrown Item collided with: " + other.gameObject.name);
        if (other.gameObject.tag.Equals(StringManager.Tags.floor) && m_GenerateNoiseOnGroundContact)
        {
            MakeNoiseAtCurrentPosition();
        }

    }
}
