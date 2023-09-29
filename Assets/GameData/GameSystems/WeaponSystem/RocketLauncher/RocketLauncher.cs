using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketLauncher : Weapon
{
    [Header("Weapon type")]
    [SerializeField] WeaponType _weaponType;
    public override WeaponType weaponType => _weaponType;

    [Header("Bullet config")]
    [SerializeField] GameObject _bulletPrefab;
    [SerializeField] Transform _bulletSpawnPoint;

    [Header("Sounds")]
    [SerializeField] AudioSource _audioSource;
    [SerializeField] List<AudioClip> _fireSounds;


    // Config data
    float _coolDown;
    float _damagePoints;

    // Private data
    bool _isShootingContinuesly;
    float _currentCoolDown;
    


    public override void SetGunStats()
    {
        _coolDown = 1;
        _damagePoints = 1;
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
        if (_fireSounds == null || _fireSounds.Count <= 0)
        {
            return;
        }

        int randomIndex = Random.Range(0, _fireSounds.Count);
        _audioSource.clip = _fireSounds[randomIndex];
        _audioSource.Play();
    }
}