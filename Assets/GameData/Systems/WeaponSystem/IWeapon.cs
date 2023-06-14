using UnityEngine;

public interface IWeapon
{
    public WeaponType weaponType {get;}
    
    public void SetGunStats();
    public void StartShootingContinuesly();
    public void StopShootingContinuesly();
    public void ShootTheGun();
}

public abstract class Weapon : MonoBehaviour, IWeapon
{
    public virtual WeaponType weaponType => throw new System.NotImplementedException();

    public virtual void SetGunStats() {}
    public virtual void StartShootingContinuesly() {}
    public virtual void StopShootingContinuesly() {}
    public virtual void ShootTheGun() {}
}