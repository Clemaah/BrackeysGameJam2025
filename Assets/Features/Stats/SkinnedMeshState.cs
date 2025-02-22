using UnityEngine;

[RequireComponent(typeof(SkinnedMeshRenderer))]
public class SkinnedMesh : MonoBehaviour
{
    private SkinnedMeshRenderer _mesh;
    public MaterialSO material;
    public BoolSO isVisible;
    
    void Start()
    {
        _mesh = GetComponent<SkinnedMeshRenderer>();
        _mesh.enabled = isVisible.value;
        material.OnValueChanged += ChangeMaterial;
        isVisible.OnValueChanged += newValue => _mesh.enabled = newValue;
        
        ChangeMaterial(material.value);
    }

    private void ChangeMaterial(Material newMaterial)
    {
        if (!material.value) return;
        
        _mesh.material = material.value;
    }
}