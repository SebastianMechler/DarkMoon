using UnityEngine;
using System.Collections;

public class PlayerObjectInteraction : MonoBehaviour
{
    public Material m_outlineMaterial = null;

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == StringManager.Tags.interactableObject)
        {
          if (IsNextObjectInteractable() == false)
            return;


            // Enable the ObjectInteraction and pass the data which will be used to highlight the object
            ObjectInteractionData data;
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

  /// <summary>
  /// This function will create a ray cast from player to to forward direction of camera
  /// It will prevent a player to interact with a object through a wall
  /// </summary>
  /// <returns></returns>
  bool IsNextObjectInteractable()
  {
    Ray ray = new Ray(this.transform.position, this.transform.rotation * Vector3.forward);
    RaycastHit hit;
    //Debug.DrawLine(this.transform.position, this.transform.rotation * Vector3.forward * 100.0f, Color.red);
    if (Physics.Raycast(ray, out hit, 300.0f))
    {
      //Debug.Log("Hit Interactable object" + hit.transform.gameObject.name.ToString());
      if (hit.transform.gameObject.tag == StringManager.Tags.interactableObject)
        return true;
    }

    return false;
  }
}
