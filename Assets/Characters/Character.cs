using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected CharacterController CharacterController;
    
    public StatSO speed;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        
    }
}
