using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;

public class Snake : Enemy
{

    public ProjectileLauncher projectileLauncher;
    
    private void FixedUpdate()
    {
        if (IsDashing) return;
        if (!target) return;
        Vector3 relativeTargetPosition = target.transform.position - transform.position;
        if (relativeTargetPosition.magnitude > detectionRadius.Get()) return;

        TryDash(Vector3.zero);
        
        transform.forward = relativeTargetPosition;
        characterController.Move(relativeTargetPosition.normalized.X0Z() * (math.remap(12.0f, 16.0f, -1.0f, 1.0f, relativeTargetPosition.magnitude) * speed.Get() * Time.deltaTime));
    }

    protected override void DashStart()
    {
        base.DashStart();
        Animator.SetBool("Attack", true);
        if (!target) return;
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return Tween.To(0.5f, 0.0f, 0.5f, f => transform.forward = target.transform.position + target.characterController.velocity * f - transform.position);
        projectileLauncher.TryFire();
        yield return null;
    }

    protected override void DashEnd()
    {
        base.DashEnd();
        Animator.SetBool("Attack", false);
    }
}
