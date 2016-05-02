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

public class ObjectInteraction : MonoBehaviour
{
    public ObjectInteractionType m_type;

    private bool m_isEnabled = false;
    private Color m_materialColorBackup;
    private GameObject m_interactionSprite = null;

    void Update()
    {
        if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.interactWithObject))
        {
            if (m_isEnabled)
            {
                Interact();
            }
        }
    }

    void Interact()
    {
#if DEBUG
        Debug.Log("Interacting with object: " + this.gameObject.name + " -- Type: " + m_type.ToString());
#endif
        switch (m_type)
        {
            case ObjectInteractionType.DOOR:
                // play opening animation
                // play opening sound
                // ...
                break;

            case ObjectInteractionType.CHEST:
                break;

            case ObjectInteractionType.MAIN_TERMINAL:
                break;

            case ObjectInteractionType.TERMINAL_ONE:
                break;

            case ObjectInteractionType.TERMINAL_TWO:
                break;

            case ObjectInteractionType.TERMINAL_THREE:
                break;
        }
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

        m_isEnabled = true;
    }

    public void Disable()
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material.color = m_materialColorBackup;

        Destroy(m_interactionSprite);

        m_isEnabled = false;
    }

    void OnTriggerExit(Collider other)
    {
        Disable();
    }
}
