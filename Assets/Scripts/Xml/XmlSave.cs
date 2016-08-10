using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

public class XmlSave : MonoBehaviour
{
  public const string m_fileName = "save.xml";

  XmlDocument m_document = new XmlDocument();

  static bool m_isCreated = false;

  string[] terminals = new string[5];

  public GameObject m_door;

  void Awake()
  {
    if (m_isCreated)
    {
      Destroy(this.gameObject);
    }
    m_isCreated = true;

    DontDestroyOnLoad(this.gameObject);

    terminals[0] = XmlNodes.Terminal.mainTerminal;
    terminals[1] = XmlNodes.Terminal.terminalOne;
    terminals[2] = XmlNodes.Terminal.terminalTwo;
    terminals[3] = XmlNodes.Terminal.terminalThree;
    terminals[4] = XmlNodes.Terminal.terminalGenerator;
  }

  void Start()
  {
    m_door = GameObject.Find("door_simple_(animated) (9)");
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F3))
    {
      Save();
    }

    if (Input.GetKeyDown(KeyCode.F4))
    {
      // Loads the xml file, if it does not exist it will create an xml tree with default values
      LoadXml(m_fileName);
      LoadGameData();
    }
  }

  public XmlNode GetXmlNode(string name, XmlElement element = null)
  {
    XmlNodeList list;

    if (element == null)
    {
      list = m_document.DocumentElement.GetElementsByTagName(name);
    }
    else
    {
      list = element.GetElementsByTagName(name);
    }

    if (list.Count == 0)
      return null;
    else
    {
      return list[0];
    }
  }

  public void Save(string file = m_fileName)
  {
    if (m_document.DocumentElement == null)
    {
      XmlElement element = m_document.CreateElement("xml");
      m_document.AppendChild(element);
    }

    SavePlayer();
    SaveEnemey();
    SaveGameDifficulty();
    SaveTerminalData();

    m_document.Save(file);
    Debug.Log("XmlSave saved.");
  }

  public bool LoadXml(string file = m_fileName)
  {
    try
    {
      m_document.Load(file);


      if (m_document.DocumentElement == null)
      {
        XmlElement element = m_document.CreateElement("xml");
        m_document.AppendChild(element);
      }
    }
    catch
    {
      // file not found, ...
      return false;
    }

    Debug.Log("XmlSave loaded.");
    return true;
  }

  public void LoadGameData()
  {
    LoadPlayer();
    LoadEnemy();
    LoadGameDifficulty();
    LoadTerminalData();

    // Screen.SetResolution @options

    SingletonManager.UIManager.TogglePauseMenu();
  }

  #region PlayerNode
  public void CreatePlayerNode()
  {
    // creates all nodes if they dont exist already

    XmlElement ele = (XmlElement)GetXmlNode(XmlNodes.Player.player);
    if (ele == null)
    {
      // element does not exist, create it
      ele = m_document.CreateElement(XmlNodes.Player.player);
      m_document.DocumentElement.AppendChild(ele);
    }

    XmlElement position = (XmlElement)GetXmlNode(XmlNodes.Player.position, ele);
    if (position == null)
    {
      // element does not exist, create it
      position = m_document.CreateElement(XmlNodes.Player.position);
      position.SetAttribute("X", "0.0");
      position.SetAttribute("Y", "0.0");
      position.SetAttribute("Z", "0.0");
      ele.AppendChild(position);
    }

    XmlElement rotation = (XmlElement)GetXmlNode(XmlNodes.Player.rotation, ele);
    if (rotation == null)
    {
      // element does not exist, create it
      rotation = m_document.CreateElement(XmlNodes.Player.rotation);
      rotation.SetAttribute("X", "0.0");
      rotation.SetAttribute("Y", "0.0");
      rotation.SetAttribute("Z", "0.0");
      ele.AppendChild(rotation);
    }

    XmlElement oxygen = (XmlElement)GetXmlNode(XmlNodes.Player.oxygen, ele);
    if (oxygen == null)
    {
      // element does not exist, create it
      oxygen = m_document.CreateElement(XmlNodes.Player.oxygen);
      oxygen.InnerText = "0.0";
      ele.AppendChild(oxygen);
    }

    XmlElement battery = (XmlElement)GetXmlNode(XmlNodes.Player.battery, ele);
    if (battery == null)
    {
      // element does not exist, create it
      battery = m_document.CreateElement(XmlNodes.Player.battery);
      battery.InnerText = "0.0";
      ele.AppendChild(battery);
    }

    XmlElement flashLight = (XmlElement)GetXmlNode(XmlNodes.Player.flashLight, ele);
    if (flashLight == null)
    {
      // element does not exist, create it
      flashLight = m_document.CreateElement(XmlNodes.Player.flashLight);
      flashLight.InnerText = "False";
      ele.AppendChild(flashLight);
    }
  }

  void SavePlayer()
  {
    CreatePlayerNode();

    PlayerNode player = new PlayerNode(GetXmlNode(XmlNodes.Player.player));
    player.SetPosition(SingletonManager.Player.GetComponent<Transform>().position);

    // y rotation is from player, x rotation is from camera...
    Vector3 rotationPlayer = SingletonManager.Player.GetComponent<Transform>().eulerAngles;
    player.SetRotation(new Vector3(Camera.main.transform.eulerAngles.x, rotationPlayer.y, rotationPlayer.z));

    // Debug.Log("Camera : " + Camera.main.transform.eulerAngles.x);
    // Debug.Log("Player : " + rotationPlayer.y);

    float oxygen = SingletonManager.Player.GetComponent<PlayerOxygen>().m_current;
    player.SetOxygen(oxygen);

    float battery = SingletonManager.Player.GetComponent<PlayerBattery>().m_current;
    player.SetBattery(battery);

    player.SetFlashLight(FlashLight.GetInstance().m_hasBeenPickedUp);
  }

  void LoadPlayer()
  {
    CreatePlayerNode();

    PlayerNode player = new PlayerNode(GetXmlNode(XmlNodes.Player.player));
    SingletonManager.Player.GetComponent<Transform>().position = player.GetPosition();

    // set player-y rotation
    Vector3 rotation = player.GetRotation();
    SingletonManager.Player.GetComponent<Transform>().eulerAngles = new Vector3(0.0f, rotation.y, 0.0f);

    // set camera-x rotation, this junk doesnt work, rotation will be fucked up all the time... idk
    //Camera.main.transform.eulerAngles = new Vector3(rotation.x, 0.0f, 0.0f);

    Debug.Log("Camera: " + rotation.x.ToString());
    Debug.Log("Player: " + rotation.y.ToString());

    SingletonManager.Player.GetComponent<PlayerOxygen>().m_current = player.GetOxygen();
    SingletonManager.Player.GetComponent<PlayerBattery>().m_current = player.GetBattery();

    FlashLight.GetInstance().m_hasBeenPickedUp = player.GetFlashLight();

  }
  #endregion

  #region EnemyNode
  void CreateEnemyNode()
  {
    // creates all nodes if they dont exist already

    XmlElement ele = (XmlElement)GetXmlNode(XmlNodes.Enemey.enemy);
    if (ele == null)
    {
      // element does not exist, create it
      ele = m_document.CreateElement(XmlNodes.Enemey.enemy);
      m_document.DocumentElement.AppendChild(ele);
    }

    XmlElement position = (XmlElement)GetXmlNode(XmlNodes.Enemey.position, ele);
    if (position == null)
    {
      // element does not exist, create it
      position = m_document.CreateElement(XmlNodes.Enemey.position);
      position.SetAttribute("X", "0.0");
      position.SetAttribute("Y", "0.0");
      position.SetAttribute("Z", "0.0");
      ele.AppendChild(position);
    }

    XmlElement rotation = (XmlElement)GetXmlNode(XmlNodes.Enemey.rotation, ele);
    if (rotation == null)
    {
      // element does not exist, create it
      rotation = m_document.CreateElement(XmlNodes.Enemey.rotation);
      rotation.SetAttribute("X", "0.0");
      rotation.SetAttribute("Y", "0.0");
      rotation.SetAttribute("Z", "0.0");
      ele.AppendChild(rotation);
    }

    XmlElement lastWaypointName = (XmlElement)GetXmlNode(XmlNodes.Enemey.lastWaypointName, ele);
    if (lastWaypointName == null)
    {
      // element does not exist, create it
      lastWaypointName = m_document.CreateElement(XmlNodes.Enemey.lastWaypointName);
      lastWaypointName.InnerText = "";
      ele.AppendChild(lastWaypointName);
    }

    XmlElement nextWaypointName = (XmlElement)GetXmlNode(XmlNodes.Enemey.nextWaypointName, ele);
    if (nextWaypointName == null)
    {
      // element does not exist, create it
      nextWaypointName = m_document.CreateElement(XmlNodes.Enemey.nextWaypointName);
      nextWaypointName.InnerText = "";
      ele.AppendChild(nextWaypointName);
    }

    XmlElement movementPattern = (XmlElement)GetXmlNode(XmlNodes.Enemey.movementPattern, ele);
    if (movementPattern == null)
    {
      // element does not exist, create it
      movementPattern = m_document.CreateElement(XmlNodes.Enemey.movementPattern);
      movementPattern.InnerText = "0";
      ele.AppendChild(movementPattern);
    }

    XmlElement isHunting = (XmlElement)GetXmlNode(XmlNodes.Enemey.isHunting, ele);
    if (isHunting == null)
    {
      // element does not exist, create it
      isHunting = m_document.CreateElement(XmlNodes.Enemey.isHunting);
      isHunting.InnerText = "False";
      ele.AppendChild(isHunting);
    }

    XmlElement huntingWaypointSourceName = (XmlElement)GetXmlNode(XmlNodes.Enemey.huntingWaypointSourceName, ele);
    if (huntingWaypointSourceName == null)
    {
      // element does not exist, create it
      huntingWaypointSourceName = m_document.CreateElement(XmlNodes.Enemey.huntingWaypointSourceName);
      huntingWaypointSourceName.InnerText = "";
      ele.AppendChild(huntingWaypointSourceName);
    }

    XmlElement huntingWaypointName = (XmlElement)GetXmlNode(XmlNodes.Enemey.huntingWaypointName, ele);
    if (huntingWaypointName == null)
    {
      // element does not exist, create it
      huntingWaypointName = m_document.CreateElement(XmlNodes.Enemey.huntingWaypointName);
      huntingWaypointName.InnerText = "";
      ele.AppendChild(huntingWaypointName);
    }
  }

  void SaveEnemey()
  {
    CreateEnemyNode();

    EnemyNode enemy = new EnemyNode(GetXmlNode(XmlNodes.Enemey.enemy));
    enemy.SetEnemyData(SingletonManager.Enemy.GetComponent<EnemyAiScript>().GetSaveData());
  }

  void LoadEnemy()
  {
    CreateEnemyNode();

    EnemyNode enemy = new EnemyNode(GetXmlNode(XmlNodes.Enemey.enemy));
    SingletonManager.Enemy.GetComponent<EnemyAiScript>().SetSavedData(enemy.GetEnemyData());
  }
  #endregion

  #region GameDifficultyNode
  public void CreateGameDifficultyNode()
  {
    // creates all nodes if they dont exist already

    XmlElement ele = (XmlElement)GetXmlNode(XmlNodes.GameDifficulty.gameDifficulty);
    if (ele == null)
    {
      // element does not exist, create it
      ele = m_document.CreateElement(XmlNodes.GameDifficulty.gameDifficulty);
      m_document.DocumentElement.AppendChild(ele);
    }
  }

  public void SaveGameDifficulty()
  {
    CreateGameDifficultyNode();

    GameDifficultyNode gameDifficulty = new GameDifficultyNode(GetXmlNode(XmlNodes.GameDifficulty.gameDifficulty));
    gameDifficulty.SetGameDifficulty(SingletonManager.GameManager.m_gameDifficulty);
  }

  public void LoadGameDifficulty()
  {
    CreateGameDifficultyNode();

    GameDifficultyNode gameDifficulty = new GameDifficultyNode(GetXmlNode(XmlNodes.GameDifficulty.gameDifficulty));
    SingletonManager.GameManager.m_gameDifficulty = gameDifficulty.GetGameDifficulty();

    // update costs
    SingletonManager.Minimap.UpdateCosts();
    SingletonManager.Player.GetComponent<PlayerOxygen>().UpdateCosts();
    SingletonManager.Player.GetComponent<PlayerBattery>().UpdateCosts();
  }
  #endregion

  #region TerminalNode
  public void CreateTerminalNode()
  {
    // creates all nodes if they dont exist already
    for (int i = 0; i < terminals.Length; i++)
    {
      XmlElement ele = (XmlElement)GetXmlNode(terminals[i]);
      if (ele == null)
      {
        // element does not exist, create it
        ele = m_document.CreateElement(terminals[i]);
        m_document.DocumentElement.AppendChild(ele);
      }

      // Terminal => activated
      XmlElement activated = (XmlElement)GetXmlNode(XmlNodes.Terminal.activated, ele);
      if (activated == null)
      {
        // element does not exist, create it
        activated = m_document.CreateElement(XmlNodes.Terminal.activated);
        activated.InnerText = false.ToString();
        ele.AppendChild(activated);
      }

      // Terminal => collected
      XmlElement collected = (XmlElement)GetXmlNode(XmlNodes.Terminal.collected, ele);
      if (collected == null)
      {
        // element does not exist, create it
        collected = m_document.CreateElement(XmlNodes.Terminal.collected);
        collected.InnerText = false.ToString();
        ele.AppendChild(collected);
      }
    }
  }

  public void SaveTerminalData()
  {
    CreateTerminalNode();

    for (int i = 0; i < terminals.Length; i++)
    {
      TerminalNode terminal = new TerminalNode(GetXmlNode(terminals[i]));
      TerminalInformation terminalInformation = SingletonManager.MainTerminalController.GetTerminalInformation(i);
      terminal.SetActivated(terminalInformation.isActivated);
      terminal.SetCollected(terminalInformation.isCollected);
    }
  }

  public void LoadTerminalData()
  {
    CreateTerminalNode();

    GameObject.Find("SpawnPositions").GetComponent<EnemySpawningCheck>().ToggleRigidbodyColliders(true);
    GameObject.Find("SpawnPositions").SetActive(false);
    SingletonManager.Enemy.SetActive(true);

    for (int i = 0; i < terminals.Length; i++)
    {
      TerminalNode terminalNode = new TerminalNode(GetXmlNode(terminals[i]));

      TerminalInformation terminalInformation = new TerminalInformation();
      terminalInformation.isActivated = terminalNode.GetActivated();
      terminalInformation.isCollected = terminalNode.GetCollected();

      SingletonManager.MainTerminalController.SetTerminalInformation(i, terminalInformation);
      // SingletonManager.MainTerminalController.SetTerminalState((TerminalType)i, TerminalState.Locked);
      Terminals terminal = SingletonManager.MainTerminalController.GetTerminalByType((TerminalType)i);
      Transform displayState = terminal.m_terminal.transform.FindChild("display_2states");

      // update visual
      if (terminalInformation.isCollected)
      {
        if((TerminalType)i == TerminalType.TERMINAL_ONE && terminalInformation.isCollected)
        {
          m_door = GameObject.Find("doors (animated) (2)");
          m_door.GetComponent<DoorBehaviour>().ChangeDoorState(DoorBehaviour.DoorState.OPENING_STUCKED);
        }

        SingletonManager.MainTerminalController.SetTerminalState((TerminalType)i, TerminalState.Unlocked);
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
      }
      else if(terminalInformation.isActivated)
      {
        SingletonManager.MainTerminalController.SetTerminalState((TerminalType)i, TerminalState.Unlocked);
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));

        if((TerminalType)i == TerminalType.TERMINAL_GENERATOR)
        {
          m_door = GameObject.Find("door_simple_(animated) (9)");
          m_door.GetComponent<DoorBehaviour>().ChangeDoorState(DoorBehaviour.DoorState.OPENING_STUCKED);
        }
        
      }else 
      {
        SingletonManager.MainTerminalController.SetTerminalState((TerminalType)i, TerminalState.Locked);
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.5f));
      }
    }

    Terminals t = SingletonManager.MainTerminalController.GetTerminalByType(TerminalType.TERMINAL_TWO);
    Transform d = t.m_terminal.transform.FindChild("display_2states");
    if (SingletonManager.MainTerminalController.GetTerminalInformation((int)TerminalType.TERMINAL_TWO).isCollected == true)
    {
      d.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
    }
    else
    {
      d.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
    }
  }
  #endregion

  
  public static XmlSave GetInstance()
  {
    return GameObject.Find(StringManager.Names.xmlSave).GetComponent<XmlSave>();
  }
  
}
