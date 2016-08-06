using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class DoorBehaviour : MonoBehaviour
{

  public enum DoorType
  {
    NONE,
    SMALL,
    LARGE
  }

  public enum SoundType
  {
    SILENT,
    NORMAL,
    NOISY,
    BROKEN,
    STUCKED
  }

  public enum DoorState
  {
    OPENING,
    OPEN,
    CLOSING,
    CLOSED,
    OPENING_BROKEN,
    BROKEN,
    OPENING_STUCKED,
    STUCKED
  }

  public DoorType m_DoorType = DoorType.NONE;
  public SoundType m_SoundType = SoundType.SILENT;
  private DoorState m_DoorState = DoorState.CLOSED;
  private const string m_AnimationName = "Take 001";
  private Animator m_Animator = null;
  private float m_Timer = 0.0f;

  public AudioSource m_AudioSourceSilent;
  public AudioSource m_AudioSourceNormal;
  public AudioSource m_AudioSourceNoisy;
  public Color m_OpenColor = Color.cyan;
  public Color m_OpeningColor = Color.green;
  public Color m_ClosedColor = Color.red;
  public Color m_ClosingColor = Color.yellow;
  public Color m_StuckedColor = Color.magenta;
  public Color m_BrokenColor = Color.grey;
  public GameObject[] m_LightGameObjects = null;
  private int m_LightCount = 0;
  private Light[] m_Lights = null;

  // doesn't work for whatever reasons
  private Vector3 m_OriginalPosition;
  private Vector3 m_AnimationPosition;
  private bool m_IsAnimating;
  private float m_Step = 0.0f;

  public void OpeningDoor()
  {
    if (m_DoorState == DoorState.OPEN || m_DoorState == DoorState.OPENING)
    {
      return;
    }

    m_Animator.SetTrigger("triggerExitAnimation");
    ChangeDoorState(DoorState.OPENING);
    // m_AudioSource.clip = Resources.Load<AudioClip>("");
  }

  public DoorState getDoorState()
  {
    return m_DoorState;
  }

  public void ChangeDoorState(DoorState state)
  {
    m_DoorState = state;

    if (m_DoorState == DoorState.OPENING)
    {
      // ChangeLightColor(m_OpeningColor);
      if (m_DoorType == DoorType.SMALL) {
        SetAnimationData(0.5f, 1.0f); 
        /*m_IsAnimating = true;*/
      }
      if (m_DoorType == DoorType.LARGE)
      {
        SetAnimationData(0.75f, 1.0f);
      }
      m_Animator.SetTrigger("triggerDoorOpening");
    }
  }

  // Use this for initialization
  void Start()
  {
    m_Animator = GetComponent<Animator>();
    // m_AudioSource = GetComponent<AudioSource>();

    m_Step = transform.position.y;
    m_OriginalPosition = transform.position;
    m_AnimationPosition = new Vector3(m_OriginalPosition.x, m_OriginalPosition.y + 3.0f, m_OriginalPosition.z);

    m_LightCount = m_LightGameObjects.Length;
    m_Lights = new Light[m_LightCount];
    for (int i = 0; i < m_LightCount; i++)
    {
      m_Lights[i] = m_LightGameObjects[i].GetComponent<Light>();
      m_Lights[i].color = m_ClosedColor;
    }
  }

  bool AnimateSimpleDoorCompleted()
  {
    float target = m_OriginalPosition.y + 2.95f;
    if (transform.position.y >= target)
    {
      return true;
    }

    transform.position = Vector3.Lerp(transform.position, m_AnimationPosition, Time.deltaTime);
    Debug.Log(gameObject.name + " >> Move Y: " + transform.position.y);

    return false;
  }

  void OnTriggerEnter(Collider other)
  {
    if (m_DoorState == DoorState.CLOSED || m_DoorState == DoorState.CLOSING)
    {
      if (other.gameObject.tag.Equals(StringManager.Tags.enemy))
      {
        ChangeDoorState(DoorState.OPENING);
      }
    }
  }

  void ChangeLightColor(Color color)
  {
    for (int i = 0; i < m_LightCount; i++)
    {
      m_Lights[i].color = color;
    }
  }

  void SetAnimationData(float timer, float speed)
  {
    m_Timer = timer;
    m_Animator.speed = speed;
  }

  // Update is called once per frame
  void Update()
  {
    //if (m_IsAnimating == true)
    //{
    //  if (AnimateSimpleDoorCompleted() == true)
    //  {
    //    m_IsAnimating = false;
    //  }
    //  else
    //  {
    //    return;
    //  }
    //}

    if (Input.GetKeyDown(KeyCode.Keypad0) && m_DoorState == DoorState.CLOSED)
    {
      ChangeDoorState(DoorState.OPENING);
    }

    if (Input.GetKeyDown(KeyCode.Keypad1) && m_DoorState == DoorState.CLOSED)
    {
      ChangeDoorState(DoorState.OPENING_BROKEN);
    }

    if (Input.GetKeyDown(KeyCode.Keypad2) && m_DoorState == DoorState.CLOSED)
    {
      ChangeDoorState(DoorState.OPENING_STUCKED);
    }

    if (m_Timer > 0.0f)
    {
      m_Timer -= Time.deltaTime;
      return;
    }

    switch (m_DoorState)
    {
      case DoorState.OPENING:
        //Debug.Log("DoorState.OPENING");
        m_DoorState = DoorState.OPEN;
        if (m_DoorType == DoorType.SMALL) SetAnimationData(4.0f, 0.0f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(4.0f, 0.0f);
        ChangeLightColor(m_OpenColor);
        break;

      case DoorState.OPEN:
        //Debug.Log("DoorState.OPEN");
        m_DoorState = DoorState.CLOSING;
        if (m_DoorType == DoorType.SMALL) SetAnimationData(0.6f, 1.0f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(1.2f, 1.0f);
        ChangeLightColor(m_ClosingColor);
        break;

      case DoorState.CLOSING:
        //Debug.Log("DoorState.CLOSING");
        m_DoorState = DoorState.CLOSED;
        if (m_DoorType == DoorType.SMALL) SetAnimationData(0.0f, 1.0f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(0.0f, 1.0f);
        m_Animator.SetTrigger("triggerExitAnimation");
        ChangeLightColor(m_ClosedColor);
        break;

      case DoorState.OPENING_BROKEN:
        //Debug.Log("DoorState.OPENING_BROKEN");
        m_Animator.SetTrigger("triggerDoorOpening");
        m_DoorState = DoorState.BROKEN;
        if (m_DoorType == DoorType.SMALL) SetAnimationData(1.0f, 0.6f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(0.75f, 1.0f);
        ChangeLightColor(m_OpenColor);
        break;

      case DoorState.BROKEN:
        ChangeLightColor(m_BrokenColor);
        if (m_DoorType == DoorType.SMALL) SetAnimationData(1.0f, 0.0f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(2.0f, 0.0f);
        this.enabled = false;
        break;

      case DoorState.OPENING_STUCKED:
        //Debug.Log("DoorState.OPENING_STUCKED");
        m_Animator.SetTrigger("triggerDoorOpening");
        m_DoorState = DoorState.STUCKED;
        if (m_DoorType == DoorType.SMALL) SetAnimationData(0.25f, 1.0f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(1.75f, 0.3f);
        ChangeLightColor(m_OpenColor);
        break;

      case DoorState.STUCKED:
        //Debug.Log("DoorState.STUCKED");
        if (m_DoorType == DoorType.SMALL) SetAnimationData(1.0f, 0.0f);
        if (m_DoorType == DoorType.LARGE) SetAnimationData(2.0f, 0.0f);
        ChangeLightColor(m_StuckedColor);
        this.enabled = false;
        break;
    }
  }
}
