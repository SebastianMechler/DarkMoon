using UnityEngine;
using System.Collections;

// TODO:
// [DONE] Check this topic to make camera not go into objects
// [DONE] Lerp rotation, to get it smooth

public class CameraRotation : MonoBehaviour
{

  public enum RotationAxes
  {
      MOUSE_X_AND_Y,
      MOUSE_X,
      MOUSE_Y
  }

  public RotationAxes m_cameraAxis = RotationAxes.MOUSE_X_AND_Y; // x-axis from screen, y-axis from screen
  public float m_sensitivityX = 15.0f; // sensity defines how fast the player can move his camera
  public float m_sensitivityY = 15.0f; // sensity defines how fast the player can move his camera
  public float m_minimumX = -360.0f; // minimum value to clamp the overall X-rotation each frame
  public float m_maximumX = 360.0f; // maximum value to clamp the overall X-rotation each frame
  public float m_minimumY = -60.0f; // minimum value to clamp the overall Y-rotation each frame
  public float m_maximumY = 60.0f; // maximum value to clamp the overall Y-rotation each frame

  private float m_rotationY = 0.0f;
  private Rigidbody m_rigidbody = null;

  void Start()
  {
    this.m_rigidbody = GetComponent<Rigidbody>();

    // Make the rigid body not change rotation
    if (m_rigidbody)
        m_rigidbody.freezeRotation = true;
  }

  void Update()
  {
    if (m_cameraAxis == RotationAxes.MOUSE_X_AND_Y)
    {
      // GET X-AXIS
      float rotX = 0.0f;
      rotX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * m_sensitivityX * Time.deltaTime;
      rotX = Mathf.Clamp(rotX, m_minimumX, m_maximumX);

      float rotYLerping = m_rotationY;
      // GET Y-AXIS
      m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY * Time.deltaTime;
      m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);
           
      // lerp Y-rotation
      rotYLerping = Mathf.Lerp(rotYLerping, m_rotationY, 0.5f);

        
      //transform.localEulerAngles = new Vector3(-rotYLerping, rotX, 0.0f);

      // set rotation on Y-axis to player
      transform.localEulerAngles = new Vector3(0.0f, rotX, 0.0f);

      // set rotation on Y-axis to camera ==> else the player would move down too on Y-Axis so theres a double
      Vector3 camRot = Camera.main.transform.localEulerAngles;
      camRot.x = -rotYLerping;
      Camera.main.transform.localEulerAngles = camRot;
    }
    else if (m_cameraAxis == RotationAxes.MOUSE_X)
    {
      // GET X-AXIS
      float rotX = 0.0f;
      rotX = Input.GetAxis("Mouse X") * m_sensitivityX * Time.deltaTime;
      rotX = Mathf.Clamp(rotX, m_minimumX, m_maximumX);
      transform.Rotate(new Vector3(0.0f, rotX, 0.0f));
    }
    else if (m_cameraAxis == RotationAxes.MOUSE_Y)
    {
      // GET Y-AXIS
      m_rotationY += Input.GetAxis("Mouse Y") * m_sensitivityY * Time.deltaTime;
      m_rotationY = Mathf.Clamp(m_rotationY, m_minimumY, m_maximumY);

      transform.localEulerAngles = new Vector3(-m_rotationY, transform.localEulerAngles.y, 0.0f);
    }
  }
}

