using UnityEngine;
using System.Collections;

public class ObjectInteractionChest : ObjectInteractionBase
{

  [Tooltip("[0.0f to max] Defines the amount of oxygen the player will obtain when opening the chest.")]
  public float m_oxygen = 60.0f;

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
    Debug.Log("Interacting with chest: " + this.gameObject.name + " -> Found " + m_oxygen.ToString() + " oxygen.");
#endif

    SingletonManager.Player.GetComponent<PlayerOxigen>().Increase(m_oxygen);

    Disable();
    Destroy(this.gameObject);
    /*
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
    */
  }
}
