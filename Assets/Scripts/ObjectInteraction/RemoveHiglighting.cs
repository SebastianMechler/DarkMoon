using UnityEngine;
using System.Collections;

public class RemoveHiglighting : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        Renderer renderer = this.gameObject.GetComponent<Renderer>();
        renderer.material = null;
    }
}
