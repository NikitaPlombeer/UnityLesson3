using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationPrinter : MonoBehaviour
{
    private void Update()
    {
        Debug.Log("Rotation: " + transform.rotation);
        Debug.Log("Euler Angles: " + transform.eulerAngles);

        Debug.Log("Local Rotation: " + transform.localRotation);
        Debug.Log("Local Euler Angles: " + transform.localEulerAngles);
    }
}
