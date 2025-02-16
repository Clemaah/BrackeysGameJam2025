using System;
using UnityEngine;

public class Character : MonoBehaviour
{
    protected CharacterController CharacterController;
    
    public FloatValue speed;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
    }

    void Update()
    {
        
    }
}
