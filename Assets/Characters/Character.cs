using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public CharacterController CharacterController;
    protected Animator Animator;
    
    public FloatValue speed;

    public float dashDelay;
    public FloatValue dashForce;
    public FloatValue dashDuration;
    public FloatValue dashCooldown;
    private float _nextDash;
    
    protected bool IsDashing;

    private void Awake()
    {
        CharacterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
    }

    public bool TryDash(Vector3 direction)
    {
        if (Time.time < _nextDash) return false;
        _nextDash = Time.time + dashCooldown.Get();
        StartCoroutine(Dash(direction, dashForce.Get(), dashDuration.Get()));
        return true;
    }

    IEnumerator Dash(Vector3 direction, float force, float duration)
    {
        DashStart();
        yield return new WaitForSeconds(dashDelay);
        float frequency = 0.016f;
        for (float t = 0.0f; t < duration; t += frequency)
        {
            float f = force * frequency / duration;
            CharacterController.Move(direction * f);
            yield return new WaitForSeconds(frequency);
        }
        DashEnd();
        yield return null;
    }

    protected virtual void DashStart()
    {
        IsDashing = true;
    }
    protected virtual void DashEnd()
    {
        IsDashing = false;
    }
}
