using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageMaterial : MonoBehaviour
{
    public MaterialSO material;
    private Image _image;
    
    void Start()
    {
        _image = gameObject.GetComponent<Image>();
        material.OnValueChanged += ChangeMaterial;
        
        ChangeMaterial(material.value);
    }

    private void ChangeMaterial(Material newMaterial)
    {
        if (!material.value) return;
        _image.material = newMaterial;
    }
}