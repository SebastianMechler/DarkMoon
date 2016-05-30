using UnityEngine;
using System.Collections;

public class CollisionDetection : MonoBehaviour {

    void OnTriggerEnter(Collider other)
    {
        if (StringManager.Tags.player.Equals(other.gameObject.tag))
        {
            GameManager.ClearDebugConsole();
            Debug.Log(" †††† The Player just died a bit ††††");
        }
    }

}
