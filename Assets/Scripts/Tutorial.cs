using UnityEngine;
using System.Collections;
using System.Collections.Generic; // dictionary



public enum TutorialState
{
  Camera,
  Movement,
  Interact,
  OxygenBattery,
  Door,
  None,
}

[System.Serializable]
public struct TutorialStateDelay
{
  public TutorialState m_state;
  public float m_delay;
}

public class Tutorial : MonoBehaviour
{
  public TutorialState m_currentTutorialState = TutorialState.None;
  private Dictionary<TutorialState, bool> m_isTutorialStateDone = new Dictionary<TutorialState, bool>();

  private float m_cameraMovementDone = 10.0f;
  private float m_currentCameraMovement = 0.0f;

  private float m_runTimer = 0.35f;

  private float m_tutorialStateTimer = 0.0f;

  public List<TutorialStateDelay> m_tutorialStateDelay; // this delay will be used between each state

  void Start()
  {
    // dictionary must be initialized with default values...
    for (int i = 0; i < (int)TutorialState.None; i++)
    {
      m_isTutorialStateDone.Add((TutorialState)i, false);
    }
  }

	void RunTutorial()
  {
	  if (SingletonManager.GameManager.m_isTutorial && SingletonManager.GameManager.m_isSaveGame == false)
    {
      // TUTORIAL LEVEL
      // Disable Movement
      SetPlayerMovementState(false);
      SetBatteryAndOxygenState(false);
      SetPlayerInteractState(false);
      
      // start tutorial with camera
      m_currentTutorialState = TutorialState.Camera;
      SetTutorialStateDelay(GetTutorialStateDelay(m_currentTutorialState));

      SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.TutorialCamera); // camera
      Debug.Log("Starting tutorial...");
    }
	}
	
	void Update ()
  {
    // just a delay on first run => so no Singleton errors occour
    if (m_runTimer > 0.0f)
    {
      m_runTimer -= Time.deltaTime;
      if (m_runTimer < 0.0f)
      {
        m_runTimer = 0.0f;
        RunTutorial();
      }
    }

    UpdateTutorialStateTimer();
    
    // update tutorial state
    switch (m_currentTutorialState)
    {
      case TutorialState.Camera:
        m_currentCameraMovement += (Input.GetAxis("Mouse Y") + Input.GetAxis("Mouse X"));
        if (m_currentCameraMovement >= m_cameraMovementDone)
        {
          if (m_isTutorialStateDone[m_currentTutorialState] == false)
          {
            SetTutorialStateDone(m_currentTutorialState);
          }          
        }
        break;

      case TutorialState.Movement:
        if (Input.GetKey(SingletonManager.GameManager.m_gameControls.forward) ||
          Input.GetKey(SingletonManager.GameManager.m_gameControls.backward) ||
          Input.GetKey(SingletonManager.GameManager.m_gameControls.left) ||
          Input.GetKey(SingletonManager.GameManager.m_gameControls.right))
        {
          if (m_isTutorialStateDone[m_currentTutorialState] == false)
          {
            SetTutorialStateDone(m_currentTutorialState);
          }
        }
        break;

      case TutorialState.Interact:
        if (FlashLight.GetInstance().m_hasBeenPickedUp)
        {
          // player looked at interaction object
          if (m_isTutorialStateDone[m_currentTutorialState] == false)
          {
            SetTutorialStateDone(m_currentTutorialState);
          }
        }
        break;
      case TutorialState.OxygenBattery:
        if (m_isTutorialStateDone[m_currentTutorialState] == false)
        {
          // Add blink effect
          SetTutorialStateDone(m_currentTutorialState);
        }
        break;

      case TutorialState.Door:
        if (m_isTutorialStateDone[m_currentTutorialState] == false)
        {
          SetTutorialStateDone(m_currentTutorialState);
        }
        break;
    }
	}

  void SetPlayerMovementState(bool state)
  {
    SingletonManager.Player.GetComponent<PlayerMovement>().enabled = state;
  }

  void SetBatteryAndOxygenState(bool state)
  {
    SingletonManager.Player.GetComponent<PlayerOxygen>().enabled = state;
    SingletonManager.Player.GetComponent<PlayerBattery>().enabled = state;
  }

  void SetPlayerInteractState(bool state)
  {
    Camera.main.GetComponent<PlayerObjectInteraction>().enabled = state;
  }

  void SetTutorialStateDone(TutorialState state)
  {
    m_isTutorialStateDone[state] = true;
  }

  void UpdateTutorialStateTimer()
  {
    // wait timer is used to set delay between TutorialStates
    if (m_tutorialStateTimer > 0.0f)
    {
      m_tutorialStateTimer -= Time.deltaTime;
    }

    if (m_tutorialStateTimer <= 0.0f)
    {
      m_tutorialStateTimer = 0.0f;

      // dont do anything when None is set
      if (m_currentTutorialState == TutorialState.None)
      {
        return;
      }

      // delay between tutorialState done
      if (m_isTutorialStateDone[m_currentTutorialState] == true)
      {
        // set next tutorial state
        switch (m_currentTutorialState)
        {
          case TutorialState.Camera:
              SetPlayerMovementState(true);
              SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.TutorialMovement); // movement
            break;

          case TutorialState.Movement:
              SetPlayerInteractState(true);
              SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.TutorialInteractWithFlashLight); // interact
            break;

          case TutorialState.Interact:
            SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.TutorialOxygenAndBattery); // oxygenBattery
            break;
          case TutorialState.OxygenBattery:
            SetBatteryAndOxygenState(true);
            SingletonManager.TextToSpeech.DoTextToSpeech(TextToSpeechType.TutorialDoor); // door
            break;

          case TutorialState.Door:
            break;
        }
        
        // set new tutorial state
        m_currentTutorialState++;

        Debug.Log("Setting new tutorial State: " + m_currentTutorialState.ToString());

        // set timer based on current tutorial state
        SetTutorialStateDelay(GetTutorialStateDelay(m_currentTutorialState));
      }
    }
  }

  void SetTutorialStateDelay(float delay)
  {
    m_tutorialStateTimer = delay;
  }

  float GetTutorialStateDelay(TutorialState state)
  {
    for (int i = 0; i < m_tutorialStateDelay.Count; i++)
    {
      if (m_tutorialStateDelay[i].m_state == state)
      {
        return m_tutorialStateDelay[i].m_delay;
      }
    }

    return 0.0f;
  }
}
