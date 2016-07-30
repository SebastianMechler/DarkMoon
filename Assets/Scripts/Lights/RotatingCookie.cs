using UnityEngine;
using System.Collections;

public class RotatingCookie : MonoBehaviour
{
    public float m_Speed = 8.0f;

    // Update is called once per frame
    void Update ()
    {
        transform.Rotate(Vector3.forward, m_Speed * Time.deltaTime);
    }
}
