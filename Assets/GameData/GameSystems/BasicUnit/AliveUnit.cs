using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AliveUnit : MonoBehaviour, IDamageble
{
    [HideInInspector] public float Health;
    [HideInInspector] public float Armour;

    public virtual void Die() {}
    public virtual void TakeDamage(float damagePoints) {}
    public virtual void TakeDamage(float damagePoints, DamageType gamageType) {}
}