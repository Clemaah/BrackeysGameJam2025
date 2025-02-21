using System;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : Character
{
    public Character target;
    
    public float detectionRadius = 24.0f;

    protected void Start()
    {
        target = FindFirstObjectByType<MainCharacter>();
    }
    
}
