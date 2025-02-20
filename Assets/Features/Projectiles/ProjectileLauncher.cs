using UnityEngine;

public class ProjectileLauncher : MonoBehaviour
{
    public Projectile projectile;
    
    public FloatValue fireRate;
    private float _nextFire;
    public FloatValue accuracy;
    public FloatValue projectileCount;
    
    public FloatValue recoil;

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
        
        // knock-back
        if (Mathf.Abs(recoil.Get()) > 0.01f)
        {
            Character character = transform.parent.GetComponent<Character>();
            if (character)
                    character.Push(-transform.forward, recoil.Get());
        }
        return true;
    }
}
