using UnityEngine;
using System.Collections;

public class XmlLoadGameData : MonoBehaviour
{
  private float m_delayTime = 0.25f; // delay is required, else pause menu won't work properly

  void Update()
  {
    if (m_delayTime > 0.0f)
      m_delayTime -= Time.deltaTime;

    if (m_delayTime < 0.0f)
    {
      m_delayTime = 0.0f;

      if (SingletonManager.GameManager.m_isSaveGame)
      {
        SingletonManager.XmlSave.LoadGameData();
      }
      Debug.Log("IsSaveGame: " + SingletonManager.GameManager.m_isSaveGame);
    }
  }
}
