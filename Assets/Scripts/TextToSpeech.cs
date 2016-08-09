using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic; // list

public enum TextToSpeechType
{
  Welcome,
  FlashLightPickup,
  Interact,

  TutorialCamera,
  TutorialMovement,
  TutorialInteractWithFlashLight,
  TutorialOxygenAndBattery,
  TutorialDoor,
  MainFrame,
}

[System.Serializable]
public struct TextToSpeechData
{
  public TextToSpeechType m_TextToSpeechType;
  public AudioType m_audioType;
  public string m_text;
}

public class TextToSpeech : MonoBehaviour
{
  public List<TextToSpeechData> m_textToSpeechList;

  private GameObject m_textToSpeech = null;
  private Text m_text = null;
  private Image m_image = null;

  private float m_disableTimer = 0.0f;

  private AudioSource m_currentAudioSource = null;

	void Start ()
  {
    m_textToSpeech = SingletonManager.UIManager.GetTextToSpeech();
    m_image = m_textToSpeech.GetComponent<Image>();
    m_text = m_textToSpeech.GetComponentInChildren<Text>();
    //DoTextToSpeech(TextToSpeechType.Welcome);
  }
	
	void Update ()
  {
	  if (m_disableTimer > 0.0f)
    {
      m_disableTimer -= Time.deltaTime;

      if (m_disableTimer < 0.0f)
      {
        m_disableTimer = 0.0f;
        SetVisibility(false);
        //SingletonManager.BGMixer.IncreaseVolume();
      }
    }
	}

  void SetVisibility(bool isVisible)
  {
    m_image.enabled = isVisible;
    m_text.enabled = isVisible;
  }

  public void ResetTextToSpeech()
  {
    m_disableTimer = 0.01f;
  }

  public float DoTextToSpeech(TextToSpeechType type, float displayTime = -1.0f)
  {
    for (int i = 0; i < m_textToSpeechList.Count; i++)
    {
      if (m_textToSpeechList[i].m_TextToSpeechType == type)
      {
        if (m_currentAudioSource != null)
        {
          m_currentAudioSource.Stop();
          m_currentAudioSource = null;
        }

        // lower background-music-volume
        if (m_disableTimer == 0.0f)
        {
          SingletonManager.BGMixer.LowerVolume();
        }

        // set text
        m_text.text = m_textToSpeechList[i].m_text;

        
        // display time
        if (displayTime == -1.0f)
        {
          // set timer to time of audiolength
          m_disableTimer = SingletonManager.AudioManager.GetPlayLengthFromType(m_textToSpeechList[i].m_audioType);
        }
        else
        {
          m_disableTimer = displayTime;
        }      

        // play audio
        //m_currentAudioSource = SingletonManager.AudioManager.Play(m_textToSpeechList[i].m_audioType);

        // show textospeech in mainui
        SetVisibility(true);

        return m_disableTimer;
      }
    }

    return 0.0f;
  }

  public static TextToSpeech GetInstance()
  {
    return GameObject.Find(StringManager.Names.textToSpeech).GetComponent<TextToSpeech>();
  }
}
