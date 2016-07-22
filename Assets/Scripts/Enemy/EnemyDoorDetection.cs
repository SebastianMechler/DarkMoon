using UnityEngine;
using System.Collections;

public class EnemyDoorDetection : MonoBehaviour
{

    private DoorBehaviour m_DoorBehaviour = null;

	// Use this for initialization
	void Start ()
	{
	    m_DoorBehaviour = transform.parent.gameObject.GetComponent<DoorBehaviour>();
	}

    void OnTriggerEnter(Collider other)
    {
        DoorBehaviour.DoorState m_DState = m_DoorBehaviour.getDoorState();
        if (m_DState == DoorBehaviour.DoorState.CLOSED || m_DState == DoorBehaviour.DoorState.CLOSING)
        {
            if (other.gameObject.tag.Equals(StringManager.Tags.enemy))
            {
                Debug.Log("Open Connected Door");
                m_DoorBehaviour.OpeningDoor();
            }
        }
    }

}
