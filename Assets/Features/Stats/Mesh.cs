using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class Mesh : MonoBehaviour
{
    public BoolSO isVisible;
    public MaterialSO material;
    public int materialID;
    public bool changeAllMaterials;
    
    private MeshRenderer _mesh;
    
    void Start()
    {
        _mesh = gameObject.GetComponent<MeshRenderer>();
        _mesh.enabled = isVisible.value;
        material.OnValueChanged += ChangeMaterial;
        isVisible.OnValueChanged += newValue => _mesh.enabled = newValue;
        
        ChangeMaterial(material.value);
    }

    private void ChangeMaterial(Material newMaterial)
    {
        if (!material.value) return;
        
        List<Material> newMaterials = new List<Material>();
        _mesh.GetMaterials(newMaterials);
        
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
        _mesh.SetMaterials(newMaterials);
    }
}