using UnityEngine;
using System.Collections;

public class XmlNodes
{
  public class Player
  {
    public const string player = "Player";
    public const string rotation = "Rotation";
    public const string position = "Position";
    public const string battery = "Battery";
    public const string oxygen = "Oxygen";
  }

  public class Enemey
  {
    public const string enemy = "Enemy";
    public const string rotation = "Rotation";
    public const string position = "Position";
  }

  public class GameDifficulty
  {
    public const string gameDifficulty = "GameDifficulty";
  }

  public class Terminal
  {
    public const string terminalOne = "TerminalOne";
    public const string terminalTwo = "TerminalTwo";
    public const string terminalThree = "TerminalThree";

    public const string activated = "IsActivated";
    public const string collected = "IsCollected";
  }
}

