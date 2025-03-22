using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Hole : MonoBehaviour
{
    private ShapeType shapeType;
    public int desiredRotationIndex;
    [DoNotSerialize] public ShapeType.Shape shape;

    private void Start()
    {
        shapeType = GetComponent<ShapeType>();
        shape = shapeType.shape;
        Debug.Log("Hole shape: " + shape);
    }
}
