﻿using UnityEngine;

public class BGMixer : MonoBehaviour
{

  private GameObject m_Player;
  private GameObject m_Enemy;
  public float m_HecticThreshold = 20.0f;

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

  public float m_BgmFactor = 1.0f;

  [Range(0.05f, 0.5f)]
  public float m_MusicChangeFactor = 0.25f;

  private float m_CounterDown = 0.0f;
  private float m_CounterUp = 1.0f;

  public float m_textToSpeechFactor = 1.0f;

  public float m_RefreshRate = 5.0f;
  private float m_RefreshCount = 5.0f;

  void Start()
  {

    m_BgmFactor = SingletonManager.GameManager.m_settings.m_musicVolume;

    m_Player = GameObject.FindGameObjectWithTag(StringManager.Tags.player);
    m_Enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);

    m_TrackSilent.Play();
    m_TrackSilent.volume = 1.0f * m_BgmFactor;

    m_TrackMystic.Play();
    m_TrackMystic.volume = 0.0f;

    m_TrackHectic.Play();
    m_TrackHectic.volume = 0.0f;

    m_TrackDanger.Play();
    m_TrackDanger.volume = 0.0f;

    m_AudioActive = ActiveAudio.Silent;
    m_AudioNext = ActiveAudio.None;

    if (m_Enemy == null)
    {
      GetComponent<BGMixer>().enabled = false;
    }
  }

  void CheckBehaviour()
  {
    if (m_RefreshCount >= 0.0f)
    {
      m_RefreshCount -= Time.deltaTime;
      return;
    }
    m_RefreshCount = m_RefreshRate;

    float distance = Mathf.Abs(Vector3.Distance(m_Player.transform.position, m_Enemy.transform.position));
    float speed = m_Enemy.GetComponent<EnemyAiScript>().getMovementSpeed();

    if (m_AudioNext != ActiveAudio.None)
    {
      return;
    }

    if (speed > 10.0f)
    {
      if (m_AudioActive != ActiveAudio.Danger && m_AudioActive != ActiveAudio.Danger)
      {
        Debug.Log("[!] Auto Change to: ActiveAudio.Danger");
        m_AudioNext = ActiveAudio.Danger;
        m_CounterDown = 1.0f;
        m_CounterUp = 0.0f;
      }
    }
    else if (distance <= m_HecticThreshold && m_AudioActive != ActiveAudio.Hectic)
    {
      Debug.Log("[!] Auto Change to: ActiveAudio.Hectic");
      m_AudioNext = ActiveAudio.Hectic;
      m_CounterDown = 1.0f;
      m_CounterUp = 0.0f;
    }
    else
    {
      if (distance > m_HecticThreshold && speed < 10.0f && m_AudioActive != ActiveAudio.Silent)
      {
        Debug.Log("[!] Auto Change to: ActiveAudio.Silent");
        m_AudioNext = ActiveAudio.Silent;
        m_CounterDown = 1.0f;
        m_CounterUp = 0.0f;
      }
    }

  }

  #region keyInput
  void CheckKeyDown()
  {
    if (Input.GetKeyDown(KeyCode.Keypad1))
    {
      if (m_AudioActive != ActiveAudio.Silent)
      {
        // Debug.Log("Change to: ActiveAudio.Silent");
        m_AudioNext = ActiveAudio.Silent;
        m_CounterDown = 1.0f;
        m_CounterUp = 0.0f;
      }
    }

    if (Input.GetKeyDown(KeyCode.Keypad2))
    {
      if (m_AudioActive != ActiveAudio.Mystic)
      {
        // Debug.Log("Change to: ActiveAudio.Mystic");
        m_AudioNext = ActiveAudio.Mystic;
        m_CounterDown = 1.0f;
        m_CounterUp = 0.0f;
      }
    }

    if (Input.GetKeyDown(KeyCode.Keypad3))
    {
      if (m_AudioActive != ActiveAudio.Hectic)
      {
        // Debug.Log("Change to: ActiveAudio.Hectic");
        m_AudioNext = ActiveAudio.Hectic;
        m_CounterDown = 1.0f;
        m_CounterUp = 0.0f;
      }
    }

    if (Input.GetKeyDown(KeyCode.Keypad4))
    {
      if (m_AudioActive != ActiveAudio.Danger)
      {
        // Debug.Log("Change to: ActiveAudio.Danger");
        m_AudioNext = ActiveAudio.Danger;
        m_CounterDown = 1.0f;
        m_CounterUp = 0.0f;
      }
    }
  }
  #endregion

  // Update is called once per frame
  void Update()
  {
    if (m_CounterDown <= 0.0f || m_CounterUp >= 1.0f)
    {
      // CheckKeyDown();
      CheckBehaviour();
    }

    if (SingletonManager.Tutorial.m_isEnabled)
    {
      return;
    }

    if (m_AudioActive != m_AudioNext && m_AudioNext != ActiveAudio.None)
    {
      m_CounterDown -= Time.deltaTime * m_MusicChangeFactor;
      m_CounterUp += Time.deltaTime * m_MusicChangeFactor;

      if (m_CounterDown < 0.0f || m_CounterUp > 1.0f)
      {
        m_CounterDown = 0.0f;
        m_CounterUp = 1.0f;
      }

      switch (m_AudioNext)
      {
        case ActiveAudio.Silent:
          m_TrackSilent.volume = (m_TrackSilent.volume <= 1.0f ? m_CounterUp : 1.0f) * m_BgmFactor;
          break;

        case ActiveAudio.Mystic:
          m_TrackMystic.volume = (m_TrackMystic.volume <= 1.0f ? m_CounterUp : 1.0f) * m_BgmFactor;
          break;

        case ActiveAudio.Hectic:
          m_TrackHectic.volume = (m_TrackHectic.volume <= 1.0f ? m_CounterUp : 1.0f) * m_BgmFactor;
          break;

        case ActiveAudio.Danger:
          m_TrackDanger.volume = (m_TrackDanger.volume <= 1.0f ? m_CounterUp : 1.0f) * m_BgmFactor;
          break;

        default:
          Debug.LogError("Undefined NEXT Audio Source Type");
          break;
      }

      switch (m_AudioActive)
      {
        case ActiveAudio.Silent:
          m_TrackSilent.volume = (m_TrackSilent.volume >= 0.0f ? m_CounterDown : 0.0f) * m_BgmFactor;
          break;

        case ActiveAudio.Mystic:
          m_TrackMystic.volume = (m_TrackMystic.volume >= 0.0f ? m_CounterDown : 0.0f) * m_BgmFactor;
          break;

        case ActiveAudio.Hectic:
          m_TrackHectic.volume = (m_TrackHectic.volume >= 0.0f ? m_CounterDown : 0.0f) * m_BgmFactor;
          break;

        case ActiveAudio.Danger:
          m_TrackDanger.volume = (m_TrackDanger.volume >= 0.0f ? m_CounterDown : 0.0f) * m_BgmFactor;
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

  public void LowerVolume()
  {
    m_textToSpeechFactor = 0.1f;
    m_TrackSilent.volume *= m_textToSpeechFactor;
    m_TrackMystic.volume *= m_textToSpeechFactor;
    m_TrackHectic.volume *= m_textToSpeechFactor;
    m_TrackDanger.volume *= m_textToSpeechFactor;
  }

  public void IncreaseVolume()
  {
    m_TrackSilent.volume /= m_textToSpeechFactor;
    m_TrackMystic.volume /= m_textToSpeechFactor;
    m_TrackHectic.volume /= m_textToSpeechFactor;
    m_TrackDanger.volume /= m_textToSpeechFactor;
    m_textToSpeechFactor = 1.0f; // if you change this, make sure to change when music changed!!
  }

  public static BGMixer GetInstance()
  {
    return GameObject.Find(StringManager.Names.bgmixer).GetComponent<BGMixer>();
  }
}
