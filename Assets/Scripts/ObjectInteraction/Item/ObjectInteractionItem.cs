using UnityEngine;
using System.Collections;

public class ObjectInteractionItem : ObjectInteractionBase
{
  public Item m_item;

  void Start()
  {
    InitializeBase(ObjectInteractionType.ITEM);
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

    switch(m_item.m_type)
    {
      case ItemType.KEY:
        inventory.AddItem(m_item);
        break;

      case ItemType.FLASH_LIGHT:
        FlashLight.GetInstance().SetPickup();
        inventory.AddItem(m_item);
        break;
    }

    Disable();
    Destroy(this.gameObject);

  }
}

