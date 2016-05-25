﻿using UnityEngine;
using System.Collections;

public class PlayerObjectInteraction : MonoBehaviour
{
  public Material m_outlineMaterial = null; // not used yet..
  public GameObject m_currentInteractingObject = null; // current object we are interacting with
  public float m_interactionDistance = 2.0f; // ray distance for object interaction
    /*
    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == StringManager.Tags.interactableObject)
        {
          if (IsNextObjectInteractable() == false)
            return;


            // Enable the ObjectInteraction and pass the data which will be used to highlight the object
            ObjectInteractionData data;
            data.m_interactionObject = other.gameObject;
            data.m_material = m_outlineMaterial;

            ObjectInteractionBase objectBase = other.gameObject.GetComponent<ObjectInteractionBase>();
            if (objectBase == null)
            {
              Debug.LogWarning("Interaction with object could happen, but no script is attachted to it: " + other.gameObject.name);
            }
            else
            {
              other.gameObject.GetComponent<ObjectInteractionBase>().Enable(data);
            }
        }
    }
    */

  void Update()
  {
    GameObject go = GetNextInteractableObject();
    if (go != null)
    {
      // if m_go is present
      // if equal, do nothing
      // if not equal -> disable first

      if (go != m_currentInteractingObject)
      {
        // disable the old one if present
        if (m_currentInteractingObject != null)
          m_currentInteractingObject.GetComponent<ObjectInteractionBase>().Disable();

        // save the new one
        m_currentInteractingObject = go;

        // enable the new one
        ObjectInteractionData data;
        data.m_material = m_outlineMaterial;
        go.GetComponent<ObjectInteractionBase>().Enable(data);
      }


    }
    else
    {
      if (m_currentInteractingObject != null)
      {
        m_currentInteractingObject.GetComponent<ObjectInteractionBase>().Disable();
        m_currentInteractingObject = null;
      }
    }
  }

  GameObject GetNextInteractableObject()
  {
    Ray ray = new Ray(this.transform.position, this.transform.rotation * Vector3.forward);

    RaycastHit hit;
    Debug.DrawLine(this.transform.position, this.transform.rotation * Vector3.forward * m_interactionDistance, Color.red);
    if (Physics.Raycast(ray, out hit, m_interactionDistance))
    {
      //Debug.Log("Hit Interactable object" + hit.transform.gameObject.name.ToString());
      if (hit.transform.gameObject.tag == StringManager.Tags.interactableObject)
        return hit.transform.gameObject;
    }

    return null;
  }

  /// <summary>
  /// This function will create a ray cast from player to to forward direction of camera
  /// It will prevent a player to interact with a object through a wall
  /// </summary>
  /// <returns></returns>
  /*
  bool IsNextObjectInteractable()
  {
    Ray ray = new Ray(this.transform.position, this.transform.rotation * Vector3.forward);
    RaycastHit hit;
    Debug.DrawLine(this.transform.position, this.transform.rotation * Vector3.forward * m_interactionDistance, Color.red);
    if (Physics.Raycast(ray, out hit, m_interactionDistance))
    {
      //Debug.Log("Hit Interactable object" + hit.transform.gameObject.name.ToString());
      if (hit.transform.gameObject.tag == StringManager.Tags.interactableObject)
        return true;
    }

    return false;
  }
  */
}
