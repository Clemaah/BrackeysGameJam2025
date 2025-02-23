using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileLauncher : MonoBehaviour
{
    private AudioSource _audioSource;
    
    public Character character;
    public Projectile projectile;
    
    public FloatValue fireRate;
    private float _nextFire;
    public FloatValue accuracy;
    public FloatValue projectileCount;
    
    public FloatValue recoil;
    
    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

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

        if (_audioSource)
        {
            _audioSource.pitch = Random.Range(0.8f, 1.2f);
            _audioSource.volume = 0.5f + n * 0.1f;
            _audioSource.Play();
        }
        
        // knock-back
        if (Mathf.Abs(recoil.Get()) > 0.01f && character)
            character.Push(-transform.forward, recoil.Get());
        
        return true;
    }
}
