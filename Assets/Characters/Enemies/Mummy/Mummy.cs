using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Mummy : Enemy
{

    public GameObject meteor;
    
    private void FixedUpdate()
    {
        if (IsDashing) return;
        if (!target) return;
        Vector3 relativeTargetPosition = target.transform.position - transform.position;
        if (relativeTargetPosition.magnitude > detectionRadius) return;

        TryDash(Vector3.zero);
        
        transform.forward = relativeTargetPosition;
        characterController.Move(relativeTargetPosition.normalized * (math.remap(4.0f, 8.0f, -0.125f, 1.0f, relativeTargetPosition.magnitude) * speed.Get() * Time.deltaTime));
    }

    protected override void DashStart()
    {
        base.DashStart();
        Animator.SetBool("Attack", true);
        if (!target) return;
        StartCoroutine(SpawnMeteor());
    }

    private IEnumerator SpawnMeteor()
    {
        yield return new WaitForSeconds(0.625f);
        GameObject go = Instantiate(meteor, target.transform.position + target.characterController.velocity * 0.125f + Vector3.up * 4.0f, Quaternion.LookRotation(Vector3.down));
        yield return new WaitForSeconds(0.125f);
        yield return Tween.To(0.5f, go.transform.position, go.transform.position + Vector3.down * 5.0f,
            t => go.transform.position = t,
            easeType: Tween.EaseType.EaseInCubic);
        go.GetComponent<Damager>().enabled = true;
        yield return null;
        Destroy(go);
        yield return null;
    }

    protected override void DashEnd()
    {
        base.DashEnd();
        Animator.SetBool("Attack", false);
    }
}
