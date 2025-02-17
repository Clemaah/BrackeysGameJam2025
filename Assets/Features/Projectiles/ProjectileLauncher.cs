using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile projectile;
    
    public FloatValue fireRate;
    private float _nextFire;

    public bool TryFire()
    {
        if (Time.time < _nextFire) return false;
        _nextFire = Time.time + (1.0f / fireRate.Get());
        Instantiate(projectile, transform.position, transform.rotation);
        return true;
    }
}
