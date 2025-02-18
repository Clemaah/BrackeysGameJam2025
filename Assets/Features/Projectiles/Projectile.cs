using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public FloatValue speed;
    public FloatValue distanceMax;
    public FloatValue scale;

    private void Start()
    {
        Invoke(nameof(DestroyProjectile), distanceMax.Get() / speed.Get());
        transform.localScale = new Vector3(scale.Get(), scale.Get(), scale.Get());
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
