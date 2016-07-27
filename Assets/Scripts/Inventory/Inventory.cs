using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;


public enum ItemType
{
    Key,
    FlashLight,
    SnapLight,
    ToolWrench,
    NONE,
}

[System.Serializable]
public class Item
{
    public ItemType m_type;
    public string m_name;
    public float m_throwForce;
    public bool m_isThrowable;
    private GameObject m_gameObject;
    
    public Item()
    {
        m_type = ItemType.NONE;
        m_name = "Unused";
        m_gameObject = null;
    }

    public Item(ItemType type, string name)
    {
        m_type = type;
        m_name = name;
    }

    public void SetGameObject(GameObject gameObject)
    {
        m_gameObject = gameObject;
    }

    public GameObject GetGameObject()
    {
        return m_gameObject;
    }

    public void SetEnableState(bool state)
    {
        if (m_gameObject == null)
        {
            Debug.Log("Item::SetEnableState gameObject is null");
        }

        m_gameObject.SetActive(state);
    }

    public void Throw()
    {
        if (m_gameObject == null)
        {
            Debug.Log("Item::Throw gameObject is null");
        }

        m_gameObject.GetComponent<ThrowItem>().Throw(m_throwForce);
    }
}

public class Inventory : MonoBehaviour
{
    public List<Item> m_itemList = new List<Item>();
    private Image m_InventoryIcon = null;
    private Image m_InventoryIconWrench = null;

  void Start()
  {
    m_InventoryIcon = GameObject.Find(StringManager.Names.inventoryIconSnapLight).GetComponent<Image>();
    m_InventoryIconWrench = GameObject.Find(StringManager.UI.inventoryIconWrench).GetComponent<Image>();
  }

    void Update()
    {
        // TESTING PURPOSE ONLY
        if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.throwItem))
        {
            ThrowFirstItem();
        }

      //DisplayLast();
      UpdateIconInUI();
    }

  public void UpdateIconInUI()
  {
    if (IsItemInInventory(ItemType.ToolWrench))
    {
      m_InventoryIconWrench.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
      m_InventoryIconWrench.sprite = Resources.Load<Sprite>(StringManager.Names.iconWrench);
    }
    else
    {
     m_InventoryIconWrench.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    }

      if (IsItemInInventory(ItemType.SnapLight))
      {
        m_InventoryIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
        m_InventoryIcon.sprite = Resources.Load<Sprite>(StringManager.Names.iconSnapLight);
      }
      else
      {
        m_InventoryIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
      }

    }

    public void DisplayLast()
    {
        if (m_itemList.Count == 0)
        {
            m_InventoryIcon.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
        }
        else
        {
            switch (m_itemList[0].m_type)
            {
                case ItemType.SnapLight:
                    m_InventoryIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    m_InventoryIcon.sprite = Resources.Load<Sprite>(StringManager.Names.iconSnapLight);
                    break;

                case ItemType.ToolWrench:
                    m_InventoryIcon.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                    m_InventoryIcon.sprite = Resources.Load<Sprite>(StringManager.Names.iconWrench);
                    break;
            }
        }
    }

    public void AddItem(Item item)
    {
        Debug.Log("Added item with name: " + item.m_name);

        if (!IsItemInInventory(item))
        {
            m_itemList.Add(item);
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < m_itemList.Count; i++)
        {
            if (m_itemList[i].GetHashCode() == item.GetHashCode())
            {
                m_itemList.RemoveAt(i);
            }
        }
    }

    public void RemoveItem(string name)
    {
        for (int i = 0; i < m_itemList.Count; i++)
        {
            if (m_itemList[i].m_name == name)
            {
                m_itemList.RemoveAt(i);
            }
        }
    }

    public bool IsItemInInventory(ItemType type)
    {
        foreach (Item item in m_itemList)
        {
            if (item.m_type == type)
            {
                return true;
            }
        }

        return false;
    }

    public bool IsItemInInventory(Item item)
    {
        foreach (Item itemFromList in m_itemList)
        {
            if (itemFromList.GetHashCode() == item.GetHashCode())
            {
                return true;
            }
        }

        return false;
    }

    public bool IsItemInInventory(string name)
    {
        foreach (Item itemFromList in m_itemList)
        {
            if (itemFromList.m_name == name)
            {
                return true;
            }
        }

        return false;
    }

    public void ThrowFirstItem()
    {
        if (m_itemList.Count == 0)
            return;

        Item item = m_itemList[0];
        item.GetGameObject().transform.position = Camera.main.transform.position; //+ (Camera.main.transform.forward * 1.5f);
        item.SetEnableState(true);
        item.Throw();
        RemoveItem(item);
    }
}
