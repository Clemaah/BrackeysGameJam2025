using UnityEngine;

public class CameraController : MonoBehaviour
{
    private MainCharacter _target;
    public float distance;
    public float roughness;
    
    void Start()
    {
        _target = FindAnyObjectByType<MainCharacter>();
    }

    void LateUpdate()
    {
        
        transform.position = Vector3.Lerp(transform.position, _target.transform.position - transform.forward * distance, Time.deltaTime * roughness);
    }
}
