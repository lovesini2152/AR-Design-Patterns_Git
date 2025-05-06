using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float followDistance = 1.5f;
    public float heightOffset = 0.0f;
    public float followSpeed = 5.0f;
    public bool faceUser = true;

    private Transform cam;

    void Start()
    {
        cam = Camera.main.transform;
    }

    void LateUpdate()
    {
        if (cam == null) return;

      
        Vector3 targetPos = cam.position + cam.forward * followDistance;
        targetPos.y += heightOffset;

     
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);

  
        if (faceUser)
        {
            Quaternion targetRot = Quaternion.LookRotation(transform.position - cam.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * followSpeed);
        }
    }
}