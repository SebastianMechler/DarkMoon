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
#if DEBUG
    Debug.Log("Interacting with door: " + this.gameObject.name);
#endif

    switch (m_doorType)
    {
      case DoorType.NORMAL:
        Open();
        break;

      case DoorType.SECURITY:
        break;

      case DoorType.AIRLOCK:
        Open();
        break;
    }


  }

  public void Open()
  {
    if (m_isOpened)
    {
      //m_animation.Play(StringManager.Animations.Door.close);
      ToBeDeletedLater_OpenDoorByRotating(90);
      m_isOpened = false;
    }
    else
    {
      //m_animation.Play(StringManager.Animations.Door.open);
      ToBeDeletedLater_OpenDoorByRotating(-90);
      m_isOpened = true;
    }

    UpdateDoorState();
  }


  void UpdateDoorState()
  {
    if (m_isOpened)
    {
      // green
      Transform doorStateChild = this.transform.parent.FindChild(StringManager.Names.doorState);
      Renderer renderer = doorStateChild.GetComponent<Renderer>();
      renderer.material.color = Color.green;
    }
    else
    {
      // red
      Transform doorStateChild = this.transform.parent.FindChild(StringManager.Names.doorState);
      Renderer renderer = doorStateChild.GetComponent<Renderer>();
      renderer.material.color = Color.red;
    }
  }
  

  void ToBeDeletedLater_OpenDoorByRotating(float angle)
  {
    Vector3 rotateAroundPos = this.transform.parent.FindChild("DoorBorderLeft").transform.position;

    this.transform.RotateAround(rotateAroundPos, Vector3.up, angle);
  }
}
