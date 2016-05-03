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
    }
}
