using UnityEngine;
using System.Collections;

public class ObjectInteractionAirlockSwitch : ObjectInteractionBase
{

  public GameObject m_door;

	void Start ()
  {
    InitializeBase(ObjectInteractionType.DOOR_SWITCH);
    m_door.GetComponent<ObjectInteractionDoor>().SetInteractionState(false);
  }
	
	void Update ()
  {
    UpdateBase();
	}

  public override void Interact()
  {
    if (m_door == null)
    {
      Debug.Log("The switch cant access the door because it is null. Check the switch if he has the GameObject door added in Inspector.");
      return;
    }

    SetInteractionState(false);
    Disable();

    m_door.GetComponent<ObjectInteractionDoor>().Open();
    m_door.GetComponent<ObjectInteractionDoor>().SetInteractionState(true);
  }
}
