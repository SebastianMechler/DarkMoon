using UnityEngine;
using System.Collections;


public struct ObjectInteractionData
{
    public Material m_material;
}


public enum ObjectInteractionType
{
    DOOR,
    CHEST,
    MAIN_TERMINAL,
    TERMINAL_ONE,
    TERMINAL_TWO,
    TERMINAL_THREE
}

public class ObjectInteractionBase : MonoBehaviour
{
    protected ObjectInteractionType m_type;

    protected bool m_isInteracting = false;
    protected Color m_materialColorBackup;
    protected Animation m_animation;
    //private GameObject m_interactionSprite = null;

    public void UpdateBase()
    {
        if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.interactWithObject))
        {
            if (m_isInteracting)
            {
                Interact();
            }
        }
    }
    
    public void InitializeBase()
    {
        this.m_animation = GetComponent<Animation>();
    }

    // this method will be overridden by each ObjectType
    public virtual void Interact()
    {
    }

    public void Enable(ObjectInteractionData data)
    {
        // just some green visual effect
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        m_materialColorBackup = renderer.material.color;
        renderer.material.color = Color.green;

        // Create Interaction object to show some visual effect to player

        // MAKE THIS WORK, THE SPRITE NEEDS TO BE RENDERER ON TOP OF EVERYTHING!!!
        //GameObject go = (GameObject)GameObject.Instantiate(Resources.Load(StringManager.Resources.interactionObject), this.transform.position, Quaternion.identity);
        //m_interactionSprite = go;

        m_isInteracting = true;
    }

    public void Disable()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material.color = m_materialColorBackup;

        //Destroy(m_interactionSprite);

        m_isInteracting = false;
    }

    void OnTriggerExit(Collider other)
    {
      // only allow disable when the playerObjectInteraction leaves the collider
      if (other.gameObject.name == StringManager.Names.objectInteraction)
        Disable();
    }
}
