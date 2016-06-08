using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// unity cant display dictionarys in inspector, so you need to do a hack... create a list and init the map with that list..........
// fuck you unity3d

public enum AudioType
{
  INTERACT,
  PUZZLE_FAILURE,
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

  void Awake()
  {
    DontDestroyOnLoad(this.transform.gameObject);
  }

  void Start ()
  {
	  foreach (AudioStruct2 key in myList)
    {
      m_audioMap[key.type] = new AudioStruct(key.volume, key.clip);
    }
	}

  public void Play(AudioType type)
  {
    AudioSource source = Camera.main.GetComponent<AudioSource>();
    source.volume = m_audioMap[type].volume;
    source.PlayOneShot(m_audioMap[type].clip);    
  }

  public static AudioManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.audioManager).GetComponent<AudioManager>();
  }
}
