using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class FirePath : MonoBehaviour
{
    private GameObject _target;

    public float maxTimeInterval = 0.5f;
    public float spacing = 1.0f;
    public float duration = 4.0f;
    public float radius = 1.0f;
    
    public ParticleSystem vfx;
    
    private Vector3 _lastPosition;
    private float _left;
    private float _lastSpawn = float.MaxValue;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _lastPosition = _target.transform.position;
    }

    void FixedUpdate()
    {
        Vector3 targetPosition = _target.transform.position;
        Vector3 speed = targetPosition - _lastPosition;
        _lastPosition = targetPosition;
        if (speed.magnitude > 5.0f) return; // early out on teleport
        _left += speed.magnitude;
        while (_left >= 0.0f)
        {
            Vector3 p = targetPosition - speed.normalized * _left;
            _left -= spacing;
            
            StartCoroutine(SpawnFire(p));
        }

        if (Time.time - _lastSpawn > maxTimeInterval)
            StartCoroutine(SpawnFire(targetPosition));
    }

    private IEnumerator SpawnFire(Vector3 position)
    {
        _lastSpawn = Time.time;
        SphereCollider newCollider = gameObject.AddComponent<SphereCollider>();
        newCollider.enabled = false;
        newCollider.isTrigger = true;
        newCollider.radius = radius;
        newCollider.center = transform.worldToLocalMatrix * position;
        Instantiate(vfx, position, Quaternion.identity);
        Destroy(newCollider, duration);
        //Debug.DrawRay(p, Vector3.up, Color.red, duration);
        yield return new WaitForSeconds(0.5f);
        newCollider.enabled = true;
    }
}
