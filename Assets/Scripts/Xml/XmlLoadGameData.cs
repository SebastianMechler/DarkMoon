using UnityEngine;
using System.Collections;

public class XmlLoadGameData : MonoBehaviour
{
  void Start()
  {
    if (SingletonManager.GameManager.m_isSaveGame)
    {
      SingletonManager.XmlSave.LoadGameData();
    }
    Debug.Log("IsSaveGame: " + SingletonManager.GameManager.m_isSaveGame);
  }
}
