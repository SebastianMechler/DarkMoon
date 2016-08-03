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
      public const string crosshair = "Crosshair"; // sprite which will be drawn as image in the UI to show the player can interact
      public const string mainTerminalController = "MainTerminal";
      public const string uiManager = "UIManager";
      public const string player = "Player";
      public const string enemy = "Enemy";
      public const string flashLight = "FlashLight";
      public const string minimap = "MinimapCamera";
      public const string grayScaleManager = "GrayScaleManager";
      public const string inventoryIconSnapLight = "InventoryIconSnapLight";
      public const string iconWrench = "iconWrench";
      public const string iconSnapLight = "iconSnapLight";
      public const string xmlSave = "XmlSave";
      public const string textToSpeech = "TextToSpeech";
      public const string bgmixer = "BackgroundMusicMixer";
    }

    public class Tags
    {
      public const string Waypoints = "Waypoint";
      public const string interactableObject = "InteractableObject";
      public const string player = "Player";
      public const string enemy = "Enemy";
      public const string noise = "NoiseSource";
      public const string floor = "Floor";
      public const string door = "Door";
    }

    public class Layer
    {
      public const string defaultLayer = "Default";
      public const string floorLayer = "Floor";
      public const string wallLayer = "Wall";
      public const string interactableObjectLayer = "InteractableObject";
      public const string minimapLayer = "Minimap";
      public const string hidingZoneLayer = "HidingZone";
    }

    public class Resources
    {
        public const string debugLvPrototype = "LvlPrototype";
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


  public class UI
  {
    public const string MainTerminal = "MainTerminalUI";
    public const string TerminalOne = "TerminalOneUI";
    public const string TerminalTwo = "TerminalTwoUI";
    public const string TerminalThree = "TerminalThreeUI";
    public const string OxygenBackground = "OxygenBackground";
    public const string BatteryBackground = "BatteryBackground";
    public const string BatteryForgeround = "BatteryForeground";
    public const string BatteryText = "BatteryText";
    public const string PauseMenu = "PauseMenuUI";
    public const string MainMenu = "MainMenuUI";
    public const string MinimapCirclePanel = "MinimapCirclePanel";
    public const string MinimapCircle = "MinimapCircle";
    public const string MinimapCircleOutline = "MinimapCircleOutline";
    public const string MinimapRect = "MinimapRect";
    public const string HiddenState = "HiddenStateImage";
    public const string inventoryIconWrench = "InventoryIconWrench";
    public const string ToggleTerminalOne = "Toggle_TerminalOne";
    public const string ToggleTerminalTwo = "Toggle_TerminalTwo";
    public const string ToggleTerminalThree = "Toggle_TerminalThree";
    public const string TextToSpeechImage = "Image_TextToSpeech";
  }


    public class UsedScripts
    {
        public const string waypointTreeNode = "WaypointTreeNode";
    }

    public class Scenes
    {
        public const string mainMenu = "MainScene";
        public const string game = "Prototype_EnemyScript";
        public const string deathScreen = "DeathScreen";
        public const string successScreen = "SuccessScreen";
        public const string optionScreen = "OptionScreen";
    }
}

