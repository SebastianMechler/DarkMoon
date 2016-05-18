using UnityEngine;
using System.Collections;

public class BGMixer : MonoBehaviour {

	enum ActiveAudio
	{
		Silent,
		Mystic,
		Hectic,
		Danger,
		None
	}
	private ActiveAudio m_AudioActive;
	private ActiveAudio m_AudioNext;

	public AudioSource m_TrackSilent;
	public AudioSource m_TrackMystic;
	public AudioSource m_TrackHectic;
	public AudioSource m_TrackDanger;

	[Range(0.05f, 0.5f)]
	public float m_MusicChangeFactor = 0.25f;

	private float m_CounterDown;
	private float m_CounterUp;
	
	// Use this for initialization
	void Start () {


		m_TrackSilent.Play();
		m_TrackSilent.volume = 1.0f;

		m_TrackMystic.Play();
		m_TrackMystic.volume = 0.0f;

		m_TrackHectic.Play();
		m_TrackHectic.volume = 0.0f;

		m_TrackDanger.Play();
		m_TrackDanger.volume = 0.0f;

		m_AudioActive = ActiveAudio.Silent;
		m_AudioNext = ActiveAudio.None;
	}

	void CheckKeyDown()
	{
		if (Input.GetKeyDown(KeyCode.Keypad1))
		{
			if (m_AudioActive != ActiveAudio.Silent)
			{
				Debug.Log("Change to: ActiveAudio.Silent");
				m_AudioNext = ActiveAudio.Silent;
				m_CounterDown = 1.0f;
				m_CounterUp = 0.0f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Keypad2))
		{
			if (m_AudioActive != ActiveAudio.Mystic)
			{
				Debug.Log("Change to: ActiveAudio.Mystic");
				m_AudioNext = ActiveAudio.Mystic;
				m_CounterDown = 1.0f;
				m_CounterUp = 0.0f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Keypad3))
		{
			if (m_AudioActive != ActiveAudio.Hectic)
			{
				Debug.Log("Change to: ActiveAudio.Hectic");
				m_AudioNext = ActiveAudio.Hectic;
				m_CounterDown = 1.0f;
				m_CounterUp = 0.0f;
			}
		}

		if (Input.GetKeyDown(KeyCode.Keypad4))
		{
			if (m_AudioActive != ActiveAudio.Danger)
			{
				Debug.Log("Change to: ActiveAudio.Danger");
				m_AudioNext = ActiveAudio.Danger;
				m_CounterDown = 1.0f;
				m_CounterUp = 0.0f;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		CheckKeyDown();

		if(m_AudioActive != m_AudioNext && m_AudioNext != ActiveAudio.None)
		{
			m_CounterDown -= Time.deltaTime * m_MusicChangeFactor;
			m_CounterUp += Time.deltaTime * m_MusicChangeFactor;

			if(m_CounterDown < 0.0f || m_CounterUp > 1.0f)
			{
				m_CounterDown = 0.0f;
				m_CounterUp = 1.0f;
            }

			switch (m_AudioNext)
			{
				case ActiveAudio.Silent:
					m_TrackSilent.volume = (m_TrackSilent.volume <= 1.0f ? m_CounterUp : 1.0f);
                    break;

				case ActiveAudio.Mystic:
					m_TrackMystic.volume = (m_TrackMystic.volume <= 1.0f ? m_CounterUp : 1.0f);
					break;

				case ActiveAudio.Hectic:
					m_TrackHectic.volume = (m_TrackHectic.volume <= 1.0f ? m_CounterUp : 1.0f);
					break;

				case ActiveAudio.Danger:
					m_TrackDanger.volume = (m_TrackDanger.volume <= 1.0f ? m_CounterUp : 1.0f);
					break;

				default:
					Debug.LogError("Undefined NEXT Audio Source Type");
					break;
			}

			switch (m_AudioActive)
			{
				case ActiveAudio.Silent:
					m_TrackSilent.volume = (m_TrackSilent.volume >= 0.0f ? m_CounterDown : 0.0f);
					break;

				case ActiveAudio.Mystic:
					m_TrackMystic.volume = (m_TrackMystic.volume >= 0.0f ? m_CounterDown : 0.0f);
					break;

				case ActiveAudio.Hectic:
					m_TrackHectic.volume = (m_TrackHectic.volume >= 0.0f ? m_CounterDown : 0.0f);
					break;

				case ActiveAudio.Danger:
					m_TrackDanger.volume = (m_TrackDanger.volume >= 0.0f ? m_CounterDown : 0.0f);
					break;

				default:
					Debug.LogError("Undefined CURRENT Audio Source Type");
					break;
			}

			if (m_CounterDown <= 0.0f || m_CounterUp >= 1.0f)
			{
				m_AudioActive = m_AudioNext;
				m_AudioNext = ActiveAudio.None;
                Debug.Log(" == Music Changed == ");
			}
        }
    }
}
