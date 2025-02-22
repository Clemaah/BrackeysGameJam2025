using Unity.Mathematics;
using UnityEngine;

public class Car : MonoBehaviour
{
    void Start()
    {
        MainCharacter mainCharacter = FindAnyObjectByType<MainCharacter>();
        mainCharacter.transform.Find("SkeletalMesh").gameObject.SetActive(false);
        transform.SetParent(mainCharacter.transform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = quaternion.identity;
        transform.localScale = Vector3.one;
    }
}
