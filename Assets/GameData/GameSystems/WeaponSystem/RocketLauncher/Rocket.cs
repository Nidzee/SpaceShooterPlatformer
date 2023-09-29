using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    [SerializeField] float _lifeTime = 3f;
    [SerializeField] float _bulletSpeed;
    [SerializeField] Rigidbody2D _rb;
    [SerializeField] Explosion _explosionPrefab;
    
    float _damagePoints;



    public void SetStats(float damagePoints)
    {
        _damagePoints = damagePoints;
    }

    public void LaunchBullet()
    {
        Vector3 bulletDirection = transform.right;

        _rb.velocity = bulletDirection * _bulletSpeed;
        
        DelayDestruction();
    }



    void OnCollisionEnter2D(Collision2D collision) {
        GameObject.Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        DestroyBullet();
    }

    public void DestroyBullet()
    {
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