using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class GrenadeExplosion : MonoBehaviour
{
    [Header("Explosion config")]
    [SerializeField] float _sphereRadius = 2f;
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] int _eplosionDamage = 10;

    [Header("Visuals config")]
    [SerializeField] bool _showExplosionSphere = true;
    [SerializeField] GameObject _spherePrefab;
    GameObject _sphere;


    private void Start()
    {
        Explode();
        DelayDestruction();
    }

    private void Explode()
    {
        // Show sphere logic
        if (_showExplosionSphere)
        {
            _sphere = GameObject.Instantiate(_spherePrefab, transform.position, Quaternion.identity);
            float size = _sphereRadius * 2;
            _sphere.transform.localScale = new Vector3(size,size,size);
        }


        // Explosion logic
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _sphereRadius);
        foreach (var col in colliders)
        {
            var damageComponent = col.GetComponent<IDamageble>();
            if (damageComponent != null)
            {
                damageComponent.TakeDamage(_eplosionDamage);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _sphereRadius);
    }

    async void DelayDestruction()
    {
        int delayBeforeDestruction = (int)_lifeTime * 1000;
        await Task.Delay(delayBeforeDestruction);

        if (_showExplosionSphere)
        {
            Destroy(_sphere);            
        }

        Destroy(gameObject);
    }
}