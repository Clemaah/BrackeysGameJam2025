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
            TryDash(relativeTargetPosition.X0Z().normalized);
        }
            
        transform.forward = relativeTargetPosition;
        characterController.Move(relativeTargetPosition.X0Z().normalized * (math.remap(8.0f, 12.0f, -0.125f, 1.0f, relativeTargetPosition.magnitude) * speed.Get() * Time.deltaTime));
    }

    protected override void DashStart()
    {
        base.DashStart();
        //Tween.To(0.375f, 0.0f, 0.25f, f => transform.forward = target.transform.position + target.characterController.velocity * f - transform.position);
        Animator.SetBool("Attack", true);
    }

    protected override void DashEnd()
    {
        base.DashEnd();
        Animator.SetBool("Attack", false);
    }
}
