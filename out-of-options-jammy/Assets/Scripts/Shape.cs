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
    
    private void Start()
    {
        shapeType = GetComponent<ShapeType>();
        shape = shapeType.shape;
        
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (travelling)
        {
            //apply a velocity towards the destination
            Vector3 direction = (destination.position - transform.position).normalized;
            rb.velocity = direction * speed;
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("destination"))
        {
            travelling = false;
        }
    }
}


