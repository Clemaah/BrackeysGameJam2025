using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Mummy : Enemy
{

    public GameObject meteor;
    
    private GameObject _currentMeteor;

    public float prediction = 0.25f;
    public float delay = 0.5f;
    public float growingDuration = 0.5f;
    public float radius = 2.0f;
    public float delayBeforeDamage = 0.125f;
    
    private void FixedUpdate()
    {
        if (IsDashing) return;
        if (!target) return;
        Vector3 relativeTargetPosition = target.transform.position - transform.position;
        if (relativeTargetPosition.magnitude > detectionRadius.Get()) return;

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
        yield return new WaitForSeconds(delay);
        _currentMeteor = Instantiate(meteor, target.transform.position + target.characterController.velocity * prediction, Quaternion.identity);
        yield return Tween.To(growingDuration, Vector3.zero, Vector3.one * radius,
            v => _currentMeteor.transform.localScale = v,
            easeType: Tween.EaseType.EaseOutCubic);
        yield return new WaitForSeconds(delayBeforeDamage);
        _currentMeteor.GetComponent<Damager>().enabled = true;
        yield return new WaitForSeconds(0.125f);
        Destroy(_currentMeteor);
        yield return null;
    }

    protected override void DashEnd()
    {
        base.DashEnd();
        Animator.SetBool("Attack", false);
    }

    private void OnDestroy()
    {
        if (_currentMeteor)
            Destroy(_currentMeteor);
    }
}
