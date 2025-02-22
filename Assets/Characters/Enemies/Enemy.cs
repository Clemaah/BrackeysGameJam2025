using System;
using Unity.Mathematics;
using UnityEngine;

public class Enemy : Character
{
    public Character target;
    
    public FloatValue detectionRadius;

    protected void Start()
    {
        target = FindFirstObjectByType<MainCharacter>();
    }
    
}
