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
    
    private void Start()
    {
        transform.rotation = rotations[currentRotationIndex];
        shapeType = GetComponent<ShapeType>();
        shape = shapeType.shape;
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


