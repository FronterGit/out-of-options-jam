using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private InputActionReference leftAction;
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference rightAction;
    
    [SerializeField] private GameObject shape;
    private Shape shapeScript;
    
    [SerializeField] private Vector2 mouseWorldPosition;
    [SerializeField] private Vector2 screenPos;
    private Camera mainCamera;
    
    //LAYERS
    [SerializeField] private LayerMask rayPlaneLayer;
    [SerializeField] private LayerMask holeLayer;
    
    //state machine
    private enum State
    {
        Empty,
        Holding
    }
    private State state = State.Empty;
    

    private void OnEnable()
    {
        leftAction.action.Enable();
        rightAction.action.Enable();
        holdAction.action.Enable();
        
        leftAction.action.started += LeftAction;
        rightAction.action.started += RightAction;
        holdAction.action.started += HoldAction;
        holdAction.action.canceled += CancelHoldAction;
    }

    private void OnDisable()
    {
        leftAction.action.Disable();
        rightAction.action.Disable();
        holdAction.action.Disable();
        
        leftAction.action.started -= LeftAction;
        rightAction.action.started -= RightAction;
        holdAction.action.started -= HoldAction;
        holdAction.action.canceled -= CancelHoldAction;
    
    }

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        //get mouse position
        screenPos = Mouse.current.position.ReadValue();
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(screenPos);
        
        switch (state)
        {
            case State.Empty:
                break;
            case State.Holding:
                ApplyShapePosition();
                LookForHole();
                break;
        }
    }

    private void LeftAction(InputAction.CallbackContext context)
    {
        //Debug.Log("Left Action");
    }
    
    private void HoldAction(InputAction.CallbackContext context)
    {
        //Debug.Log("Hold Action");
        
        switch (state)
        {
            case State.Empty:
                LookForShape();
                break;
            case State.Holding:
                break;
        }
    }
    
    private void CancelHoldAction(InputAction.CallbackContext context)
    {
        Debug.Log("Cancel Hold Action");
        
        switch (state)
        {
            case State.Empty:
                break;
            case State.Holding:
                LoseShape();
                break;
        }
    }
    
    
    
    private void RightAction(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case State.Empty:
                break;
            case State.Holding:
                shapeScript.NextRotation();
                break;
        }
    }
    
    private void LookForShape()
    {
        var ray = mainCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 5f);
        //Debug.Log("Raycast");
        
        if (Physics.Raycast(ray, out hit, 1000, ~rayPlaneLayer))
        {
            //check tag of the object we hit
            if (hit.collider.CompareTag("shape"))
            {
                GrabShape(hit);
            }
        }
    }
    
    private void LookForHole()
    {
        var ray = mainCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.green, 5f);
        //Debug.Log("Raycast");
        
        if (Physics.Raycast(ray, out hit, 1000, holeLayer))
        {
            //check tag of the object we hit
            if (hit.collider.CompareTag("hole"))
            {
                Debug.Log("Hit hole");
            }
        }
    }

    private void GrabShape(RaycastHit hit)
    {
        shape = hit.collider.gameObject;
        shapeScript = shape.GetComponent<Shape>();
        
        //reset shape rotation
        shapeScript.currentRotationIndex = 0;
        shape.transform.rotation = shapeScript.rotations[shapeScript.currentRotationIndex];
        
        //reset any velocity the shape has
        shape.GetComponent<Rigidbody>().velocity = Vector3.zero;
        shape.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        
        state = State.Holding;
        //Debug.Log("Hit shape");
    }
    
    private void LoseShape()
    {
        //lose reference to shape
        shape = null;
        shapeScript = null;
        state = State.Empty;
    }
    
    private void ApplyShapePosition()
    {
        //raycast from the camera onto rayplane
        var ray = mainCamera.ScreenPointToRay(screenPos);
        if (Physics.Raycast(ray, out var hit, 1000, rayPlaneLayer))
        {
            shape.transform.position = hit.point;
        }
    }
}
