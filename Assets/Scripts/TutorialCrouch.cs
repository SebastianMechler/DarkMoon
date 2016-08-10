using UnityEngine;
using System.Collections;

public class TutorialCrouch : MonoBehaviour {

	// 
  void OnTriggerEnter(Collider col)
  {
    bool terminalThreeActivate = SingletonManager.MainTerminalController.GetTerminalInformation((int)TerminalType.TERMINAL_THREE).isActivated;
    if(col.gameObject.tag == StringManager.Tags.player && SingletonManager.GameManager.m_isTutorial && terminalThreeActivate)
    {
      SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.TutorialCrouch, 4.0f);
      gameObject.SetActive(false);
    }
  }
}
