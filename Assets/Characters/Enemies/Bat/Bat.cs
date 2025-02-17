using System;
using Unity.Mathematics;
using UnityEngine;

public class Bat : Enemy
{

    private void FixedUpdate()
    {
        if (IsDashing) return;
        if (!target) return;
        Vector3 relativeTargetPosition = target.transform.position - transform.position;
        if (relativeTargetPosition.magnitude > detectionRadius) return;

        if (relativeTargetPosition.magnitude < 12.0f)
        {
            TryDash(relativeTargetPosition.normalized);
        }
            
        transform.forward = relativeTargetPosition;
        CharacterController.Move(relativeTargetPosition.normalized * (math.remap(8.0f, 12.0f, -0.125f, 1.0f, relativeTargetPosition.magnitude) * speed.Get() * Time.deltaTime));
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
