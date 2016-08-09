using UnityEngine;
using System.Collections;

public class ObjectInteractionItem : ObjectInteractionBase
{
  public Item m_item;
  public GameObject m_suit;

  void Start()
  {
    InitializeBase(ObjectInteractionType.ITEM);
    m_item.SetGameObject(this.gameObject);
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
    base.Interact();
#if DEBUG
    Debug.Log("Interacting with chest: " + this.gameObject.name);
#endif

    Inventory inventory = GameObject.FindGameObjectWithTag(StringManager.Tags.player).GetComponent<Inventory>();

    // disable item visibility
    m_item.SetEnableState(false);

    // item specific things
    switch (m_item.m_type)
    {
      case ItemType.Key:
        break;

      case ItemType.FlashLight:
        FlashLight.GetInstance().SetPickup();
        //SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.FlashLightPickup);
        m_suit.SetActive(false);
        break;

      case ItemType.SnapLight:
        break;
    }

    if (m_item.m_isThrowable)
    {
      inventory.AddItem(m_item);
    }

    // disable interacting
    Disable();
  }
}

