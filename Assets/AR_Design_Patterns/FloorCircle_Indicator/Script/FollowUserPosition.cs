using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using UnityEngine.Events;

public class FollowUserPosition : MonoBehaviour
{
    private Transform cameraTransform;

    void Start()
    {
        // Attempt to find the main camera at the start
        cameraTransform = Camera.main?.transform;

        if (cameraTransform == null)
        {
            Debug.LogError("Main camera not found. Make sure your camera is tagged as MainCamera.");
        }
        else
        {
            Debug.Log("Main camera found: " + cameraTransform.name);
        }
    }

    void Update()
    {
        if (cameraTransform != null)
        {
            // Update the position of this object to match the camera's position
            transform.position = cameraTransform.position;

            // Optionally, match the rotation if you want the object to face the same direction as the user
            // transform.rotation = cameraTransform.rotation;
        }
        else
        {
            Debug.LogWarning("Camera transform is null in Update.");
        }
    }
}