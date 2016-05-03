using UnityEngine;
using System.Collections;

public enum DoorType
{
  NORMAL, // opens on interaction
  HEAVY, // opens after inserting code through Keyboard
  SWITCH // opens after a switch is pressed
}

public class ObjectInteractionDoor : ObjectInteractionBase
{
  /*
  private bool m_isOpened = false;

  void Start()
  {
    m_type = ObjectInteractionType.CHEST;
    InitializeBase();
  }

  void Update()
  {
    UpdateBase();

    if (m_isInteracting)
    {
    }
  }

  public override void Interact()
  {
    Debug.Log("ObjectInteractionDoor");


    if (m_isOpened)
    {
      m_animation.Play(StringManager.Animations.Chest.close);
      m_isOpened = false;
    }
    else
    {
      m_animation.Play(StringManager.Animations.Chest.open);
      m_isOpened = true;
    }
  }
  */
}
