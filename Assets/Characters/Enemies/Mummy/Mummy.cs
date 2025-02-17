using System;
using Unity.Mathematics;
using UnityEngine;

public class Mummy : Enemy
{
    private void FixedUpdate()
    {
        if (IsDashing) return;
        if (!target) return;
        Vector3 relativeTargetPosition = target.transform.position - transform.position;
        if (relativeTargetPosition.magnitude > detectionRadius) return;

        TryDash(Vector3.zero);
        
        transform.forward = relativeTargetPosition;
        CharacterController.Move(relativeTargetPosition.normalized * (math.remap(4.0f, 8.0f, -0.125f, 1.0f, relativeTargetPosition.magnitude) * speed.Get() * Time.deltaTime));
    }

    protected override void DashStart()
    {
        base.DashStart();
        Animator.SetBool("Attack", true);
    }

    protected override void DashEnd()
    {
        base.DashEnd();
        Animator.SetBool("Attack", false);
    }
}
