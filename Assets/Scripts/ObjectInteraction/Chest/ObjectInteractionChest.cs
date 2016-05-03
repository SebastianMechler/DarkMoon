using UnityEngine;
using System.Collections;

public class ObjectInteractionChest : ObjectInteractionBase
{
  private bool m_isOpened = false;

  void Start()
  {
    m_type = ObjectInteractionType.CHEST;
    InitializeBase();
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
    Debug.Log("ObjectInteractionChest");

    
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
}
