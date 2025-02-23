using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{
    public FloatValue scale;
    public GameObject weaponMesh;
    
    protected void Start()
    {
        if (!gameObject.activeSelf) return;
        weaponMesh.transform.localScale = Vector3.one * scale.Get();
        scale.stat.OnValueChanged += _ =>
        {
            if (!gameObject.activeSelf)
                StartCoroutine(
                    Tween.To(0.5f,
                        weaponMesh.transform.localScale,
                        Vector3.one * scale.Get(),
                        v => weaponMesh.transform.localScale = v, easeType: Tween.EaseType.EaseOutBack)
                );
        };
    }
}
