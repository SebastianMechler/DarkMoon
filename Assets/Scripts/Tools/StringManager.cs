using UnityEngine;
using System.Collections;

public class StringManager : MonoBehaviour
{
    public class Names
    {
        public const string audioManager = "AudioManager"; // NOT USED YET
        public const string mouseManager = "MouseManager";
        public const string gameManager = "GameManager";
        public const string objectInteraction = "ObjectInteraction"; // invisisble collider which is used to interact with objects
        public const string doorState = "DoorState"; // name of gameobject which indicates the current state of a door
        public const string interactionHandFromUI = "InteractionHand"; // sprite which will be drawn as image in the UI to show the player can interact
  }

    public class Tags
    {
        public const string Waypoints = "Waypoint";
        public const string interactableObject = "InteractableObject";
        public const string player = "Player";
    }

    public class Resources
    {
        public const string interactionObject = "InteractionSprite";
    }

    public class Animations
    {
        public class Chest
        {
            public const string open = "ChestAnim";
            public const string close = "ChestAnim";
        }

        public class Door
        {
          public const string open = "open";
          public const string close = "close";
        }
    }
}
