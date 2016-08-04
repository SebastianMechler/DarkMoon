using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// unity cant display dictionarys in inspector, so you need to do a hack... create a list and init the map with that list..........
// fuck you unity3d

public enum AudioType
{
  INTERACT,
  PUZZLE_FAILURE,
  PUZZLE_SUCCESS,
  TERMINAL_COMPILE_SUCCESS,
  UI_BUTTON_CLICK,
  THROW_ITEM,
  BATTERY_OUT,
  BATTERY_RECHARGE,
  OXYGEN_OUT,
  OXYGEN_RECHARGE,
  FLASHLIGHT_ON,
  FLASHLIGHT_OFF,
  PLAYER_DEATH,
  ITEM_COLLISION_TOOLWRENCH,
  TEXT_TO_SPEECH_WELCOME,
  TEXT_TO_SPEECH_FLASHLIGHT_PICKUP,
  TEXT_TO_SPEECH_INTERACT,

  TEXT_TO_SPEECH_TUTORIAL_CAMERA,
  TEXT_TO_SPEECH_TUTORIAL_MOVEMENT,
  TEXT_TO_SPEECH_TUTORIAL_INTERACT_WITH_FLASH_LIGHT,
  TEXT_TO_SPEECH_TUTORIAL_OXYGEN_AND_BATTERY,
  TEXT_TO_SPEECH_TUTORIAL_DOOR,
}

[System.Serializable]
public class AudioStruct
{
  public float volume;
  public AudioClip clip;

  public AudioStruct(float volume, AudioClip clip)
  {
    this.volume = volume;
    this.clip = clip;
  }
}

[System.Serializable]
public class AudioStruct2
{
  public AudioType type;
  public float volume = 1.0f;
  public AudioClip clip = null;
}


public class AudioManager : MonoBehaviour
{
  public List<AudioStruct2> myList;
  public Dictionary<AudioType, AudioStruct> m_audioMap = new Dictionary<AudioType, AudioStruct>();


  static bool m_isCreated = false;

  void Awake()
  {
    if (m_isCreated)
    {
      Destroy(this.gameObject);
    }
    m_isCreated = true;
    
    DontDestroyOnLoad(this.transform.gameObject);
  }

  void Start ()
  {
	  foreach (AudioStruct2 key in myList)
    {
      m_audioMap[key.type] = new AudioStruct(key.volume, key.clip);
    }
	}

  public AudioSource Play(AudioType type)
  {
    //AudioSourcePlay play = new AudioSourcePlay();
    AudioSourcePlay play = this.gameObject.AddComponent<AudioSourcePlay>();
    AudioSource audioSource = this.gameObject.AddComponent<AudioSource>();
    play.m_audioSource = audioSource;

    audioSource.clip = m_audioMap[type].clip;
    audioSource.volume = m_audioMap[type].volume * SingletonManager.GameManager.m_settings.m_soundVolume;
    audioSource.Play();

    return audioSource;

    //AudioSource source = GetComponent<AudioSource>();//Camera.main.GetComponent<AudioSource>();
    //source.volume = m_audioMap[type].volume * SingletonManager.GameManager.m_settings.m_soundVolume;

    //source.PlayOneShot(m_audioMap[type].clip);

    // create script
    // add values
    // attach script
  }

  public float GetPlayLengthFromType(AudioType type)
  {
    return m_audioMap[type].clip.length;
  }

  public static AudioManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.audioManager).GetComponent<AudioManager>();
  }
}
