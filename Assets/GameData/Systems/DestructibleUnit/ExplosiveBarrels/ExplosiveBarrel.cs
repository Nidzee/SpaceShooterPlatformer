using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : AliveUnit
{
    [Header("Main components config")]
    [SerializeField] DestructibleUnitType _unitType;
    [SerializeField] Explosion _explosionPrefab;

    [Header("Destroy sound")]
    [SerializeField] AudioClip _destroySound;




    public void Start()
    {
        // Init health data from stats
        DestructibleUnitStats stats = DestructibleUnitsSystemManager.Instance.GetDestructibleUnitStats(_unitType);
        Health = stats.maxHealthPoints;
    }

    public override void TakeDamage(float damagePoints)
    {
        if (Health <= 0)
        {
            return;
        }


        Health -= damagePoints;
        if (Health <= 0)
        {
            Die();
        }
    }
    
    void PlayDestroySound()
    {
        // SKip if sound not provided
        if (_destroySound == null)
        {
            return;
        }

        AudioSource.PlayClipAtPoint(_destroySound, transform.position);
    }

    
    
    public override void Die()
    {
        PlayDestroySound();
        GameObject.Instantiate(_explosionPrefab, transform.position, Quaternion.identity);
        Destroy(this.gameObject);
    }
}