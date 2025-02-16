using System.Collections;
using UnityEngine;


public class MainCharacter : Character
{
    public ProjectileLauncher projectileLauncher;

    public FloatValue dashForce;
    public FloatValue dashDuration;
    public FloatValue dashCooldown;
    private float _nextDash;
    
    private bool _isDashing;

    void Update()
    {
        Vector2 inputs = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));

        if (inputs.magnitude > 0.1f && (Input.GetKeyUp(KeyCode.Space) || Input.GetKeyUp(KeyCode.LeftShift)))
        {
            if (TryDash())
            {
                return;
            }
        }
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 intersection = ray.origin + ray.direction * Mathf.Abs(ray.origin.y / ray.direction.y);
        transform.forward = (intersection - transform.position).X0Z();

        if (_isDashing) return;

        CharacterController.Move(inputs.X0Y().normalized * (speed.Get() * Time.deltaTime));

        if (Input.GetMouseButton(0))
        {
            projectileLauncher.TryFire();
        }
    }

    public bool TryDash()
    {
        if (Time.time < _nextDash) return false;
        _nextDash = Time.time + dashCooldown.Get();
        StartCoroutine(Dash(CharacterController.velocity.X0Z(), dashForce.Get(), dashDuration.Get()));
        return true;
    }

    IEnumerator Dash(Vector3 direction, float force, float duration)
    {
        _isDashing = true;
        float frequency = 0.016f;
        for (float t = 0.0f; t < duration; t += frequency)
        {
            float f = force * frequency / duration;
            CharacterController.Move(direction * f);
            yield return new WaitForSeconds(frequency);
        }
        _isDashing = false;
        yield return null;
    }
}