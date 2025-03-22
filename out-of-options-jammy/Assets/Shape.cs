using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape : MonoBehaviour
{
    [SerializeField] public List<Quaternion> rotations;
    [SerializeField] public int currentRotationIndex = 0;
    
    //shape types
    public enum ShapeType
    {
        Square,
        Triangle,
        Circle
    }
    public ShapeType shapeType;
    
    private void Start()
    {
        transform.rotation = rotations[currentRotationIndex];
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
}


