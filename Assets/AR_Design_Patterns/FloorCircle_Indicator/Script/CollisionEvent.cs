using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class CollisionEvent : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onCollisionEnterEvent;

    [SerializeField]
    private UnityEvent onCollisionStayEvent;

    [SerializeField]
    private UnityEvent onCollisionExitEvent;

    [SerializeField]
    private UnityEvent onTriggerEnterEvent;

    [SerializeField]
    private UnityEvent onTriggerStayEvent;

    [SerializeField]
    private UnityEvent onTriggerExitEvent;

    // Start is called before the first frame update
    void Start()
    {
        // Check if there is any Collider attached, if not, add a SphereCollider
        if (GetComponent<Collider>() == null)
        {
            gameObject.AddComponent<SphereCollider>();
        }

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation; // Optional: Prevents rotation
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        onTriggerEnterEvent.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        onCollisionEnterEvent.Invoke();
    }


    private void OnCollisionStay(Collision collision)
    {
        onCollisionStayEvent.Invoke();
    }

    private void OnTriggerStay(Collider other)
    {
        onTriggerStayEvent.Invoke();
    }

    private void OnCollisionExit(Collision collision)
    {
        onCollisionExitEvent.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        onTriggerExitEvent.Invoke();
    }
}
