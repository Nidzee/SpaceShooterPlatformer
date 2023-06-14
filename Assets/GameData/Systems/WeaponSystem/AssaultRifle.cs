using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssaultRifle : Weapon
{
    [SerializeField] WeaponType _weaponType;
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawnPoint;

    [Header("Sounds")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _assaultRifleFireSounds;

    public override WeaponType weaponType => _weaponType;

    // Config data
    float _coolDown;
    float _damagePoints;
    float _emission;

    // Private data
    bool _isShootingContinuesly;
    float _currentCoolDown;
    


    public override void SetGunStats()
    {
        _coolDown = 0.15f;
        _damagePoints = 5;
        _emission = 0.1f;
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
        bullet.GetComponent<Bullet>().LaunchBullet(_emission);
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