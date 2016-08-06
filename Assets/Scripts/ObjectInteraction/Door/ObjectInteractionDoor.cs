using UnityEngine;
using System.Collections;

// TODO: implement SECURITY AND AIRLOCK DOOR

public enum DoorType
{
  NORMAL, // open/close on interaction
  SECURITY, // opens after inserting code through Keyboard
  AIRLOCK // opens after a switch is pressed
}

public class ObjectInteractionDoor : ObjectInteractionBase
{
  public DoorType m_doorType;
  public GameObject m_door;
  private bool m_isOpened = false;

  void Start()
  {
    InitializeBase(ObjectInteractionType.DOOR);
    UpdateDoorState();
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
    base.Interact();
#if DEBUG
    Debug.Log("Interacting with door: " + this.gameObject.name);
#endif

    switch (m_doorType)
    {
      case DoorType.NORMAL:
        Open();
        break;

      case DoorType.SECURITY:
        Debug.LogError("DoorType.SECURITY unused for:" + gameObject.name);
        break;

      case DoorType.AIRLOCK:
        Debug.LogError("DoorType.AIRLOCK unused for:" + gameObject.name);
        break;
    }


  }

  public void Open()
  {
    if (m_door.GetComponent<DoorBehaviour>().enabled)
    {
      if (m_door.GetComponent<DoorBehaviour>().getDoorState() == DoorBehaviour.DoorState.CLOSED)
      {
        m_door.GetComponent<DoorBehaviour>().ChangeDoorState(DoorBehaviour.DoorState.OPENING);
      }
    }
  }


  void UpdateDoorState()
  {

  }


  void ToBeDeletedLater_OpenDoorByRotating(float angle)
  {

  }

  public bool GetOpenState()
  {
    return m_isOpened;
  }
}
