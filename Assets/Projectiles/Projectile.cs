using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float distanceMax;

    private void Start()
    {
        Invoke(nameof(DestroyProjectile), distanceMax / speed);
    }

    private void FixedUpdate()
    {
        transform.Translate(0, 0, speed * Time.fixedDeltaTime);
    }

    public void DestroyProjectile()
    {
        Destroy(gameObject);
    }
}
