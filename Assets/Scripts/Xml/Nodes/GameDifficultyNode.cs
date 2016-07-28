using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

public class GameDifficultyNode : XmlNodeBase
{
  private XmlNode m_main = null;

  public GameDifficultyNode(XmlNode mainNode)
  {
    Initialize(mainNode);
  }

  public void Initialize(XmlNode mainNode)
  {
    m_main = mainNode;
  }

  public void SetGameDifficulty(GameDifficulty difficulty)
  {
    SetInt(m_main, (int)difficulty);
  }

  public GameDifficulty GetGameDifficulty()
  {
    return (GameDifficulty)GetInt(m_main);
  }
}


