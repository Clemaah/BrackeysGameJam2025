using System;
using UnityEngine;

public class Enemy : Character
{
    public Character target;
    
    public float detectionRadius = 24.0f;

    private void Start()
    {
        target = FindFirstObjectByType<MainCharacter>();
    }

    private void FixedUpdate()
    {
        if (!target) return;
        Vector3 relativeTargetPosition = target.transform.position - transform.position;
        if (relativeTargetPosition.magnitude > detectionRadius) return;
        transform.forward = relativeTargetPosition;
        CharacterController.Move(relativeTargetPosition.normalized * (speed.Get() * Time.deltaTime));
    }
}
