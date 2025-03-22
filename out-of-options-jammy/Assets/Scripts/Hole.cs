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
    [SerializeField] private Material outlineMaterial;
    private MeshRenderer meshRenderer;
    private Material[] materials;

    private void Start()
    {
        shapeType = GetComponent<ShapeType>();
        shape = shapeType.shape;
        Debug.Log("Hole shape: " + shape);
        meshRenderer = GetComponent<MeshRenderer>();
        materials = meshRenderer.materials;
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
}
