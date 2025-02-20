using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class Mummy : Enemy
{

    public GameObject meteor;
    
    private GameObject _currentMeteor;
    
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
        yield return new WaitForSeconds(0.5f);
        _currentMeteor = Instantiate(meteor, target.transform.position + target.characterController.velocity * 0.25f + Vector3.up * 4.0f, Quaternion.LookRotation(Vector3.down));
        yield return Tween.To(0.25f, Vector3.zero, Vector3.one,
            v => _currentMeteor.transform.localScale = v,
            easeType: Tween.EaseType.EaseOutCubic);
        yield return Tween.To(0.5f, _currentMeteor.transform.position, _currentMeteor.transform.position + Vector3.down * 5.0f,
            t => _currentMeteor.transform.position = t,
            easeType: Tween.EaseType.EaseInCubic);
        _currentMeteor.GetComponent<Damager>().enabled = true;
        yield return null;
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
