using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class HealthBat : MonoBehaviour
{
    private static readonly int NormalizedHealth = Shader.PropertyToID("_NormalizedHealth");
    
    private Damageable _damageable;
    private MeshRenderer _meshRenderer;
    private MaterialPropertyBlock _materialPropertyBlock;
    
    void Start()
    {
        _damageable = transform.parent.GetComponent<Damageable>();
        if (!_damageable)
        {
            Destroy(gameObject);
            return;
        }
        _damageable.onDamage.AddListener(UpdateBar);
        _meshRenderer = GetComponent<MeshRenderer>();
        _materialPropertyBlock = new MaterialPropertyBlock();
        
        UpdateBar();
    }

    private void OnDestroy()
    {
        _damageable.onDamage.RemoveListener(UpdateBar);
    }

    private void Update()
    {
        transform.forward = Camera.main.transform.forward;
    }

    private void UpdateBar()
    {
        _materialPropertyBlock.SetFloat(NormalizedHealth, _damageable.health / _damageable.maxHealth.Get());
        _meshRenderer.SetPropertyBlock(_materialPropertyBlock);
    }
}
