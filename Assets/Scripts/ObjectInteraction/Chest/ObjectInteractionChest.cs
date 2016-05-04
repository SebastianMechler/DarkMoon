using UnityEngine;
using System.Collections;

public class ObjectInteractionChest : ObjectInteractionBase
{
  private bool m_isOpened = false;

  void Start()
  {
    InitializeBase(ObjectInteractionType.CHEST);
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
#if DEBUG
    Debug.Log("Interacting with chest: " + this.gameObject.name);
#endif

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
