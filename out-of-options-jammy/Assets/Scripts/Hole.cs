using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class Hole : MonoBehaviour
{
    private ShapeType shapeType;
    public int desiredRotationIndex;
    [DoNotSerialize] public ShapeType.Shape shape;
    [SerializeField] private Material outlineMaterial;
    private MeshRenderer meshRenderer;
    private Material[] materials;
    int clickRequired = 1;
    int clickCount = 0;
    public bool open = true;
    
    public static event Action firstUsedEvent; 
    public bool firstUsed = false;
    
    //refs
    public TMP_Text clickCountText;

    private void Start()
    {
        shapeType = GetComponent<ShapeType>();
        shape = shapeType.shape;
        Debug.Log("Hole shape: " + shape);
        meshRenderer = GetComponent<MeshRenderer>();
        materials = meshRenderer.materials;
        clickCountText.enabled = false;
    }
    
    public void AddOutline()
    {
        //add the outline material to the list of materials of the mesh renderer
        Material[] newMaterials = new Material[materials.Length + 1];
        
        //fill newMaterials with the materials of the mesh renderer
        for (int i = 0; i < materials.Length; i++)
        {
            newMaterials[i] = materials[i];
        }
        newMaterials[^1] = outlineMaterial;
        meshRenderer.materials = newMaterials;
    }

    public void RemoveOutline()
    {
        meshRenderer.materials = materials;
    }
    
    public void OpenCover()
    {
        Debug.Log("Cover opened");
        clickRequired *= 2;
        //animate
        open = true;
        clickCountText.enabled = false;

    }

    public void buttonPress()
    {
        if (open) return;
        
        clickCount--;
        clickCountText.text = clickCount.ToString();
        if (clickCount == 0)
        {
            OpenCover();
        }
    }

    public void AcceptShape()
    {
        if (shape == ShapeType.Shape.Square)
        {
            //animate close and then open
            return;
        };
        
        open = false;
        //animate
        clickCount = clickRequired;
        clickCountText.enabled = true;
        clickCountText.text = clickCount.ToString();
        
        if (!firstUsed)
        {
            firstUsed = true;
            firstUsedEvent?.Invoke();
        }
    }
}
