using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField] float _lifeTime = 3f;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] AudioClip _explodeSound;
    [SerializeField] GrenadeExplosion _explosionPrefab;
    
    float _damagePoints;



    public void SetStats(float damagePoints)
    {
        _damagePoints = damagePoints;
    }

    public void LaunchGrenade(float throwForce)
    {
        Vector3 throwDirection = (transform.right + Vector3.up).normalized;

        _rb.AddForce(throwDirection * throwForce, ForceMode2D.Impulse);
        
        DelayDestruction();
    }



    public void DestroyBullet()
    {
        AudioSource.PlayClipAtPoint(_explodeSound, transform.position);
        GameObject.Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    async void DelayDestruction()
    {
        int delayBeforeDestruction = (int)_lifeTime * 1000;
        await Task.Delay(delayBeforeDestruction);

        if (this != null)
        {
            DestroyBullet();
        }
    }
}