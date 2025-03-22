using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] public List<Quaternion> rotations;
    [SerializeField] public int currentRotationIndex = 0;
    private ShapeType shapeType;
    public ShapeType.Shape shape;
    public bool travelling = false;
    public Transform destination;
    public Rigidbody rb;
    public float speed = 5;
    public bool falling = false;
    
    private void Start()
    {
        shapeType = GetComponent<ShapeType>();
        shape = shapeType.shape;
        
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void Update()
    {
        if (travelling)
        {
            //apply a velocity towards the Destination
            Vector3 direction = (destination.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
        if (falling)
        {
            //slowly move the shape down
            transform.position -= Vector3.up * (speed * Time.deltaTime);
        }
    }

    public void OnGrab()
    {
        //reset shape rotation
        currentRotationIndex = 0;
        transform.rotation = rotations[currentRotationIndex];
        
        //reset any velocity the shape has
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        travelling = false;
        
        //lock the rotation of the shape
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void OnLose()
    {
        //remove the rotation lock
        rb.constraints = RigidbodyConstraints.None;
    }

    public void NextRotation()
    {
        currentRotationIndex++;
        if (currentRotationIndex >= rotations.Count)
        {
            currentRotationIndex = 0;
        }
        transform.rotation = rotations[currentRotationIndex];
    }

    public void EnterHole(Vector3 position)
    {
        //remove the rigidbody component
        Destroy(rb);
        
        //collider is now a trigger
        GetComponent<Collider>().isTrigger = true;
        
        transform.position = position + Vector3.up * 3;
        falling = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destination"))
        {
            var dest = other.GetComponent<Destination>();
            if (dest.nextDestination)
            {
                rb.velocity = Vector3.zero;
                destination = dest.nextDestination.transform;
            }
            else
            {
                travelling = false;
                rb.constraints = RigidbodyConstraints.None;
            }
        }
    }
}


