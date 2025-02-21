using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class Character : MonoBehaviour
{
    [HideInInspector] public CharacterController characterController;
    protected Animator Animator;

    public FloatValue speed;
    public FloatValue scale;

    public float dashDelay;
    public FloatValue dashForce;
    public FloatValue dashDuration;
    public FloatValue dashCooldown;
    private float _nextDash;
    
    protected bool IsDashing;

    protected void Awake()
    {
        characterController = GetComponent<CharacterController>();
        Animator = GetComponentInChildren<Animator>();
    }

    protected void Start()
    {
        scale.stat.OnValueChanged += () => StartCoroutine(Tween.To(0.5f, transform.localScale, Vector3.one * scale.Get(), v => transform.localScale = v, easeType: Tween.EaseType.EaseOutBack));
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
            characterController.Move(direction * f);
            
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

    public void Push(Vector3 direction, float force)
    {
        StartCoroutine(Tween.To(Mathf.Log(force * 0.0625f + 1.0f), direction, Vector3.zero, 
            v => characterController.Move(v * (force * Time.deltaTime)), easeType: Tween.EaseType.EaseOutCubic));
    }
}
