using UnityEngine;

public class SkinnedMeshMaterialState : MonoBehaviour
{
    public SkinnedMeshRenderer mesh;
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
        
        mesh.material = material.value;
    }
}