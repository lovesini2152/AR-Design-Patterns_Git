using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]
public class WayNode : MonoBehaviour
{
    public UnityEvent eventsAfterNodeReached;
    private void Awake()
    {
        ConfigureComponents();
    }
    private void Start()
    {
        //Debug.Log(name + " " + isFirstChild() + " or " + isLastChild());
    }
    private void ConfigureComponents()
    {
        Rigidbody rb = this.GetComponent<Rigidbody>();
        BoxCollider bc = this.GetComponent<BoxCollider>();

        bc.isTrigger = true;
        rb.useGravity = false;
        rb.isKinematic = true;
        //rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private bool isFirstChild()
    {
        return this.transform.parent.GetChild(0).gameObject.Equals(this.gameObject);
    }
    private bool isLastChild()
    {
        return this.transform.parent.GetChild(transform.parent.childCount - 1).gameObject.Equals(this.gameObject); 
    }
    private void OnTriggerEnter(Collider other)
    {

        if (isFirstChild()) return;
        if (other.gameObject.tag == "MainCamera")
        {
            Debug.Log(this.gameObject + " " + other.gameObject.name);
            eventsAfterNodeReached.Invoke();
            if (isLastChild())
            {
                this.GetComponentInParent<CircleDrawer>().eventsAfterLastNodeReached.Invoke();
            }
            else
            {
                this.GetComponentInParent<CircleDrawer>().setNextNodes();
            }
        }

    }
}
