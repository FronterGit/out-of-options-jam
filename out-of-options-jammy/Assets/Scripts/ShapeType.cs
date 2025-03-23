using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShapeType : MonoBehaviour
{
    public enum Shape
    {
        Square,
        Pyramid,
        Circle,
        Bridge,
        SemiCircle,
        Star
    }
    public Shape shape;
}
