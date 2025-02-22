using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class Weapon : MonoBehaviour
{

    protected void Awake()
    {
        /*
        characterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
        transform.localScale = Vector3.one * scale.Get();
        scale.stat.OnValueChanged += _ => StartCoroutine(
            Tween.To(0.5f, 
                transform.localScale, 
                Vector3.one * scale.Get(), 
                v => transform.localScale = v, easeType: Tween.EaseType.EaseOutBack)
            );*/
    }
}
