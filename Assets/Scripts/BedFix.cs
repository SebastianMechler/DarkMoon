using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BedFix : MonoBehaviour
{
  public List<GameObject> m_bedFixList;

  public static BedFix GetInstance()
  {
    return GameObject.Find(StringManager.Names.bedFix).GetComponent<BedFix>();
  }
}
