using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[System.Serializable]
public class ColorPuzzle : MonoBehaviour
{
  public GameObject[] m_sequence;
  public GameObject[] m_sequenceFromPlayer;

	void Start ()
  {
    m_sequenceFromPlayer = new GameObject[m_sequence.Length];
	}

  public void ResetSequence()
  {
    for (int i = 0; i < m_sequence.Length; i++)
    {
      m_sequenceFromPlayer[i] = null;
    }
  }

  public void AddSequence(GameObject gameObject)
  {
    for (int i = 0; i < m_sequence.Length; i++)
    {
      if (m_sequenceFromPlayer[i] == null)
      {
        Debug.Log("Added Sequence with index: " + i + " gameObjectName: " + gameObject.name);
        m_sequenceFromPlayer[i] = gameObject;

        // end reached of playerSequence?
        if (i == m_sequence.Length - 1)
        {
          if (IsPuzzleSolved())
          {
            Debug.Log("VICTORY @ PUZZLE");
          }
          else
          {
            Debug.Log("FAILURE @ PUZZLE");
            ResetSequence();
          }
        }

        return;
      }


    }
  }

  public bool IsPuzzleSolved()
  {
    int counter = 0;
    for (int i = 0; i < m_sequence.Length; i++)
    {
      if (m_sequence[i] == m_sequenceFromPlayer[i])
      {
        counter++;
      }
    }

    if (counter == m_sequence.Length)
    {
      return true;
    }
    else
    {
      return false;
    }
  }
  


}
