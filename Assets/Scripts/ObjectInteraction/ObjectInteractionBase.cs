using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public struct ObjectInteractionData
{
  public Material m_material;
}


public enum ObjectInteractionType
{
    DOOR,
    DOOR_SWITCH,
    CHEST,
    ITEM,
    TERMINAL,
    COLOR_PUZZLE,
}

public class ObjectInteractionBase : MonoBehaviour
{
  protected ObjectInteractionType m_baseType;

  protected bool m_isInteracting = false; // will be set to true if the player is in range of this object and looks at it with the collider
  protected bool m_isAllowedToInteract = true;
  protected Color m_materialColorBackup;
  protected Animation m_animation; // reference to animation so any animation can be played
  protected ObjectInteractionData m_data;  

  public static GameObject m_interactionHand; // interactionHand which will be renderer as UI element in the game to show the player he can interact

  public void UpdateBase()
    {
        if (m_isInteracting && Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.interactWithObject))
        {
            Interact();
        }
    }
    
    public void InitializeBase(ObjectInteractionType type)
    {
        m_baseType = type;
        this.m_animation = GetComponent<Animation>();
    }

    // this method will be overridden by each ObjectType
    public virtual void Interact()
    {
      SingletonManager.AudioManager.Play(AudioType.INTERACT);
    }
    
    public void Enable(ObjectInteractionData data)
     {
        if (m_isInteracting || m_isAllowedToInteract == false)
          return;
        // just some green visual effect
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        m_materialColorBackup = renderer.material.color;
        //renderer.material.color = Color.green;

        // Create Interaction object to show some visual effect to player
        
        //GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(StringManager.Resources.interactionObject), this.transform.position, Quaternion.identity);
        m_data = data;

        SetInteractionHandState(true);

        m_isInteracting = true;
    }

    public void Disable()
    {
      if (!m_isInteracting)
        return;

      Renderer renderer = this.gameObject.GetComponent<Renderer>();
      renderer.material.color = m_materialColorBackup;

      SetInteractionHandState(false);    

      m_isInteracting = false;
    }

    void SetInteractionHandState(bool state)
    {
      if (m_interactionHand == null)
        m_interactionHand = GameObject.Find(StringManager.Names.crosshair);
      //m_interactionHand.GetComponent<Image>().enabled = true;
      
  }

  void OnTriggerExit(Collider other)
    {
      // only allow disable when the playerObjectInteraction leaves the collider
      //if (other.gameObject.name == StringManager.Names.objectInteraction)
      //  Disable();
    }
    
   public void SetInteractionState(bool state)
  {
    Debug.Log("Setting state of obj: " + this.gameObject.name.ToString() + " to:" + state.ToString());
    m_isAllowedToInteract = state;
  }

  public bool GetInteractionState()
  {
    return m_isAllowedToInteract;
  }
}
