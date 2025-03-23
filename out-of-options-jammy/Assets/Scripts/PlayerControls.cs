using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class PlayerControls : MonoBehaviour
{
    //INPUT ACTIONS
    [SerializeField] private InputActionReference leftAction;
    [SerializeField] private InputActionReference holdAction;
    [SerializeField] private InputActionReference rightAction;
    
    //SHAPE REFS
    [SerializeField] private GameObject shape;
    private Shape shapeScript;
    
    //MOUSE POSITION
    [SerializeField] private Vector2 screenPos;
    private Camera mainCamera;
    
    //LAYERS
    [SerializeField] private LayerMask rayPlaneLayer;
    [SerializeField] private LayerMask holeLayer;
    
    [SerializeField] private List<string> tutorialText = new List<string>();
    private int tutorialIndex = 0;
    [SerializeField] private TMPro.TMP_Text tutorialTextUI;
    bool fadeInText = false;
    bool fadeOutText = false;
    float fadeSpeed = 1;
    bool firstLeftAction = true;
    bool firstRightAction = true;
    
    //EVENTS
    private event Action OnLeaveHole;
    
    //VARS
    private enum State
    {
        Empty,
        Holding
    }
    private State state = State.Empty;
    private Hole hoveredHole;
    private Vector3 hoveredHolePosition;
    private bool correctHole;
    private bool controlsEnabled = false;
    private bool once = true;

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
        
        tutorialTextUI.text = tutorialText[tutorialIndex];
        fadeInText = true;
    }

    private void Update()
    {
        //get mouse position
        screenPos = Mouse.current.position.ReadValue();
        
        switch (state)
        {
            case State.Empty:
                break;
            case State.Holding:
                ApplyShapePosition();
                LookForHole();
                break;
        }
        
        if(fadeInText)
        {
            tutorialTextUI.color = new Color(tutorialTextUI.color.r, tutorialTextUI.color.g, tutorialTextUI.color.b, tutorialTextUI.color.a + Time.deltaTime * fadeSpeed);
            if (tutorialTextUI.color.a >= 1)
            {
                fadeInText = false;
            }
        }
        
        if(fadeOutText)
        {
            tutorialTextUI.color = new Color(tutorialTextUI.color.r, tutorialTextUI.color.g, tutorialTextUI.color.b, tutorialTextUI.color.a - Time.deltaTime * fadeSpeed);
            if (tutorialTextUI.color.a <= 0)
            {
                fadeOutText = false;

                if (!firstLeftAction)
                {
                    tutorialTextUI.text = tutorialText[tutorialIndex];
                    fadeInText = true;
                }
            }
        }
        
        //text follows mouse position
        tutorialTextUI.transform.position = screenPos;
    }

    private void LeftAction(InputAction.CallbackContext context)
    {
        switch (state)
        {
            case State.Empty:
                LookForButton();
                break;
            case State.Holding:
                break;
        }
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
        
        if(firstLeftAction)
        {
            fadeOutText = true;
            tutorialIndex++;
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
                if (correctHole && hoveredHole.open)
                {
                    shapeScript.EnterHole(hoveredHolePosition);
                    hoveredHole.AcceptShape();
                    LoseShape();
                }
                else LoseShape();
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
        
        if(firstRightAction)
        {
            fadeOutText = true;
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
                if (!hoveredHole)
                {
                    hoveredHole = hit.collider.GetComponent<Hole>();
                    hoveredHolePosition = hit.collider.transform.position;
                    hoveredHole.AddOutline();
                    OnLeaveHole += hoveredHole.RemoveOutline;
                }
                CheckHole(hit);
            }

        }
        else
        {
            LoseHole();
        }
    }
    
    private void LookForButton()
    {
        var ray = mainCamera.ScreenPointToRay(screenPos);
        RaycastHit hit;
        
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.blue, 5f);
        //Debug.Log("Raycast");

        if (Physics.Raycast(ray, out hit, 1000, ~rayPlaneLayer))
        {
            //check tag of the object we hit
            if (hit.collider.CompareTag("hole"))
            {
                var hole = hit.collider.GetComponent<Hole>();
                hole.buttonPress();
            }
        }
    }
    
    private void CheckHole(RaycastHit hit)
    {
        var hole = hit;
        Hole holeScript = hole.collider.GetComponent<Hole>();
        
        if (holeScript.shape == shapeScript.shape)
        {
            //if the shape is in the correct rotation
            if (holeScript.desiredRotationIndex == shapeScript.currentRotationIndex)
            {
                Debug.Log("Correct Shape and Rotation");
                correctHole = true;
            }
            else
            {
                Debug.Log("Correct Shape but Incorrect Rotation");
            }
        }
        else
        {
            Debug.Log("Incorrect Shape: " + shapeScript.shape + " Hole: " + holeScript.shape);
        }
    }

    private void GrabShape(RaycastHit hit)
    {
        shape = hit.collider.gameObject;
        shapeScript = shape.GetComponent<Shape>();

        shapeScript.OnGrab();

        
        state = State.Holding;
        //Debug.Log("Hit shape");
    }
    
    private void LoseShape()
    {
        shapeScript.OnLose();
        //lose reference to shape
        shape = null;
        shapeScript = null;
        correctHole = false;
        OnLeaveHole?.Invoke();
        hoveredHole = null;
        hoveredHolePosition = Vector3.zero;
        state = State.Empty;
        LoseHole();
    }
    
    private void LoseHole()
    {
        correctHole = false;
        OnLeaveHole?.Invoke();
        hoveredHole = null;
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
