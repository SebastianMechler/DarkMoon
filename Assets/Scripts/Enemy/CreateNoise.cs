using UnityEngine;
using System.Collections;

public class CreateNoise : MonoBehaviour {

	public string m_UniqueName;
	public bool m_PlaySoundOnEnter;

	// Use this for initialization
	void Start ()
	{
		m_UniqueName = gameObject.GetHashCode().ToString();
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == StringManager.Tags.player)
		{
			if (m_PlaySoundOnEnter)
			{
				gameObject.GetComponent<AudioSource>().Play();
			}
		}
	}
}
