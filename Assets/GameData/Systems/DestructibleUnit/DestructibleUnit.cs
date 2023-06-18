using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleUnit : AliveUnit, ICanDropItem
{
    [Header("Main components config")]
    [SerializeField] LootBag _lootBag;
    [SerializeField] DestructibleUnitType _unitType;

    [Header("Destroy sound")]
    [SerializeField] AudioClip _destroySound;


    public LootBag lootBag { get => _lootBag; }



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
    
    public void DropLoot()
    {
        lootBag?.DropLoot();
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
        DropLoot();
        
        Destroy(this.gameObject);
    }
}