using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XYZ_Rotate : MonoBehaviour
{
    // Rotational axis that can be adjusted in the Inspector
    public Vector3 rotateAxis = new Vector3(0, 1, 0); // Default to Y axis rotation
    public float rotateSpeed = 10f; // Default rotation speed

    // Update is called once per frame
    void Update()
    {
        // Rotate object by multiplying rotation speed with delta time
        transform.Rotate(rotateAxis * rotateSpeed * Time.deltaTime);
    }
}