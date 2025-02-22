using Unity.Mathematics;
using UnityEngine;

public class AttachToPlayer : MonoBehaviour
{
    public bool removeSkeletalMesh = false;
    
    public Material material;
    
    void Start()
    {
        MainCharacter mainCharacter = FindAnyObjectByType<MainCharacter>();
        transform.SetParent(mainCharacter.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = quaternion.identity;
        transform.localScale = Vector3.one;
        if (removeSkeletalMesh)
        {
            mainCharacter.transform.Find("SkeletalMesh").gameObject.SetActive(false);
        }
        else
        {
            if (material)
            {
                mainCharacter.transform.Find("SkeletalMesh").Find("Zahir").GetComponent<SkinnedMeshRenderer>().material = material;
            }
        }
    }
}
