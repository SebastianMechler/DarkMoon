using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public enum ItemType
{
  KEY,
  FLASH_LIGHT,
  NONE,
}

[System.Serializable]
public class Item
{
  public ItemType m_type;
  public string m_name;

  public Item()
  {
    m_type = ItemType.NONE;
    m_name = "Unused";
  }

  public Item(ItemType type, string name)
  {
    m_type = type;
    m_name = name;
  }
}

public class Inventory : MonoBehaviour
{
  public List<Item> m_itemList = new List<Item>();

  
  void Start()
  {
    /*
    Item item = new Item(ItemType.KEY, "LockDoorY");

    AddItem(new Item(ItemType.KEY, "LockDoorX"));
    AddItem(item);
    AddItem(new Item(ItemType.KEY, "LockDoorZ"));

    Debug.Log(m_itemList.Count);

    Debug.Log(IsItemInInventory("LockDoorZ"));
    RemoveItem(item);
    Debug.Log(IsItemInInventory(item));

    RemoveItem("LockDoorX");
    

    Debug.Log(m_itemList.Count);
    Debug.Log(m_itemList[0].m_name);
    */
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
    for (int i=0; i < m_itemList.Count; i++)
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
}
