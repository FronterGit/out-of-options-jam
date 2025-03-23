using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareHole : Hole
{
    //an event action that will be invoked when a shape is placed in the hole
    public static event System.Action OnSquarePlacedWrong;
    public static event System.Action OnSquarePlacedRight;
    private bool once;
    
    public List<ShapeType.Shape> acceptedShapes = new List<ShapeType.Shape>();
    public List<int> acceptedRotations = new List<int>();

    public void SquareHoleActivatedWrong()
    {
        //invoke the event
        OnSquarePlacedWrong?.Invoke();
    }
    
    public void SquareHoleActivatedRight()
    {
        if (once) return;
        //invoke the event
        OnSquarePlacedRight?.Invoke();
        once = true;
    }
}
