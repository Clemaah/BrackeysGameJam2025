using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public FloatValue speed;
    public FloatValue distanceMax;

    private void Start()
    {
        Invoke(nameof(DestroyProjectile), distanceMax.Get() / speed.Get());
    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, speed.Get() * Time.fixedDeltaTime);
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
