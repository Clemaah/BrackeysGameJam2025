using System.Collections.Generic;
using UnityEngine;

public class MeshMaterialState : MonoBehaviour
{
    [Header("Mesh Informations")]
    public MeshRenderer mesh;
    public int materialID;
    public bool changeAllMaterials;
    
    [Header("Current Material")]
    public MaterialSO material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        material.OnValueChanged += ChangeMaterial;
        
        ChangeMaterial(material.value);
    }

    private void ChangeMaterial(Material newMaterial)
    {
        if (!material.value) return;
        
        List<Material> newMaterials = new List<Material>();
        mesh.GetMaterials(newMaterials);
        
        if (changeAllMaterials)
        {
            for (int i = 0; i < newMaterials.Count; i++)
                newMaterials[i] = material.value;
        }
        else
        {
            for (int i = 0; i < newMaterials.Count; i++)
            {
                if (i != materialID) return;
                newMaterials[i] = material.value;
            }
        }
        mesh.SetMaterials(newMaterials);
    }
}