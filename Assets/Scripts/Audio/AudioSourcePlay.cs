using UnityEngine;
using System.Collections;

public class AudioSourcePlay : MonoBehaviour
{
  public AudioSource m_audioSource = null;

  void Update()
  {
	  if (m_audioSource.isPlaying == false)
    {
      Destroy(m_audioSource);
      Destroy(this);
    }
	}
}
