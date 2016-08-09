using UnityEngine;
using System.Collections;

public class MainframeInfo : MonoBehaviour
{

	void OnTriggerEnter(Collider other)
  {
    if (SingletonManager.GameManager.m_isTutorial && other.gameObject.tag == StringManager.Tags.player)
    {
      SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.MainFrame, 10.0f);
      this.gameObject.SetActive(false);
    }
  }
}
