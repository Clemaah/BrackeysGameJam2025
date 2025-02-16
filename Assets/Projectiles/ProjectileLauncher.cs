using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile projectile;
    
    public float fireRate;
    private float _nextFire;

    public bool TryFire()
    {
        if (Time.time < _nextFire) return false;
        _nextFire = Time.time + (1.0f / fireRate);
        Instantiate(projectile, transform.position, transform.rotation);
        return true;
    }
}
