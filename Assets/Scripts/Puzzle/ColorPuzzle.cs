using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ColorPuzzle : MonoBehaviour
{
  [Tooltip("[drag a door in] Door which will be opened after the puzzle has been solved successfully. Be sure to drag the door which has the required Door-Script attached. Usually DoorCenter.")]
  public GameObject m_doorToOpen;

  [Tooltip("[drag puzzle objects in] Defines the sequence in which the player has to interact with the objects to solve the puzzle.")]
  public GameObject[] m_sequence;

  [Tooltip("[don't touch this] Just to see what the player has solved currently in the inspector (Debug-Purposes)")]
  public GameObject[] m_sequenceFromPlayer;

	void Start ()
  {
    m_sequenceFromPlayer = new GameObject[m_sequence.Length];
    m_doorToOpen.GetComponent<ObjectInteractionDoor>().SetInteractionState(false);
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
            SingletonManager.AudioManager.Play(AudioType.PUZZLE_SUCCESS);

            // TODO:
            // open exit door

            if (m_doorToOpen == null)
            {
              Debug.Log("Make sure to attach a door to the ColorPuzzle.");
            }
            else
            {
              ObjectInteractionDoor objInteractionDoor = m_doorToOpen.GetComponent<ObjectInteractionDoor>();
              if (objInteractionDoor == null)
              {
                Debug.Log("Make sure to attach a door with the script ObjectInteractionDoor attached to it, usually DoorCenter has this script.");
              }
              else
              {
                objInteractionDoor.SetInteractionState(true);
                objInteractionDoor.Interact();
              }
            }
            
          }
          else
          {
            Debug.Log("FAILURE @ PUZZLE");
            SingletonManager.AudioManager.Play(AudioType.PUZZLE_FAILURE);
            //ShowFailure();
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

  public void ShowFailure()
  {
    foreach (Transform child in this.gameObject.transform)
    {
      child.gameObject.AddComponent<ColorPuzzleFailure>();
    }
  }

}
