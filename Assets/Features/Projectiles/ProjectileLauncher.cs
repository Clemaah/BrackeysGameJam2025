using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile projectile;
    
    public FloatValue fireRate;
    private float _nextFire;
    public FloatValue accuracy;
    public FloatValue projectileCount;

    public bool TryFire()
    {
        if (Time.time < _nextFire) return false;
        _nextFire = Time.time + (1.0f / fireRate.Get());
        float n = Mathf.Round(projectileCount.Get());
        for (float i = 0; i < n; i++)
        {
            float angle = (i - (n - 1) * 0.5f + Random.Range(-0.5f, 0.5f)) * accuracy.Get();
            Instantiate(projectile, transform.position, transform.rotation * Quaternion.Euler(Vector3.up * angle));
        }
        return true;
    }
}
