using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
{
    [Header("Weapon type")]
    [SerializeField] WeaponType _weaponType;
    public override WeaponType weaponType => _weaponType;

    [Header("Bullet config")]
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawnPoint;

    [Header("Sounds")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _assaultRifleFireSounds;


    // Config data
    float _coolDown;
    float _damagePoints;

    // Private data
    bool _isShootingContinuesly;
    float _currentCoolDown;
    


    public override void SetGunStats()
    {
        var weaponStats = WeaponSystemManager.Instance.GetWeaponStats(_weaponType , 1, 1);

        _coolDown = weaponStats.cooldown;
        _damagePoints = weaponStats.damagePoints;
    }

    public override void StartShootingContinuesly() => _isShootingContinuesly = true;

    public override void StopShootingContinuesly() => _isShootingContinuesly = false;



    void Update()
    {
        UpdateCoolDown();

        // Try to shoot
        if (_isShootingContinuesly)
        {
            if (_currentCoolDown <= 0)
            {
                ShootTheGun();
                ResetCoolDown();
            }
        }
    }

    public override void ShootTheGun()
    {
        playShootSound();
        var bullet = Instantiate(_bulletPrefab, _bulletSpawnPoint.position, _bulletSpawnPoint.rotation);
        var bulletCpmponent = bullet.GetComponent<Bullet>();
        bulletCpmponent.SetStats(_damagePoints);
        bulletCpmponent.LaunchBullet();
    }





    // Cool down logic
    void UpdateCoolDown()
    {
        if (_currentCoolDown > 0)
        {
            _currentCoolDown -= Time.deltaTime;
        }
    }

    void ResetCoolDown()
    {
        _currentCoolDown = _coolDown;
    }






    // Visuals and sounds management
    void playShootSound()
    {
        // Skip if no sounds provided
        if (_assaultRifleFireSounds == null || _assaultRifleFireSounds.Count <= 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, _assaultRifleFireSounds.Count);
        _audioSource.clip = _assaultRifleFireSounds[randomIndex];
        _audioSource.Play();
    }
}