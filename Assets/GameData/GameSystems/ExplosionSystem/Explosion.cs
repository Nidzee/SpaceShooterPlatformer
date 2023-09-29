using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion config")]
    [SerializeField] float _sphereRadius = 2f;
    [SerializeField] float _lifeTime = 1f;
    [SerializeField] int _eplosionDamage = 10;

    [Header("Knockback config")]
    [SerializeField] float _sphereKnockbackRadius;
    [SerializeField] float _knockbackForce;
    [SerializeField] private LayerMask _knockBackLayer;


    [Header("Visuals config")]
    [SerializeField] bool _showExplosionSphere = true;
    [SerializeField] GameObject _spherePrefab;
    [SerializeField] GameObject _knockbackSpherePrefab;
    GameObject _sphere;
    GameObject _knockbackSphere;


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

            if (_knockbackSpherePrefab != null)
            {
                _knockbackSphere = GameObject.Instantiate(_knockbackSpherePrefab, transform.position, Quaternion.identity);
                float knockBackSize = _sphereKnockbackRadius * 2;
                _knockbackSphere.transform.localScale = new Vector3(knockBackSize,knockBackSize,knockBackSize);
            }
        }


        // Explosion logic
        Collider2D[] damageColliders = Physics2D.OverlapCircleAll(transform.position, _sphereRadius);
        foreach (var col in damageColliders)
        {
            var damageComponent = col.GetComponent<IDamageble>();
            if (damageComponent != null)
            {
                damageComponent.TakeDamage(_eplosionDamage);
            }
        }

        Collider2D[] knockbackColliders = Physics2D.OverlapCircleAll(transform.position, _sphereKnockbackRadius);
        foreach (var col in knockbackColliders)
        {
            var rigidbody2D = col.attachedRigidbody;
            if (rigidbody2D != null)
            {
                if (( _knockBackLayer & (1 << rigidbody2D.gameObject.layer)) != 0)

                rigidbody2D.AddExplosionForce(
                    _knockbackForce, 
                    transform.position, 
                    _sphereKnockbackRadius);
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
            Destroy(_knockbackSphere);            
        }

        Destroy(gameObject);
    }
}