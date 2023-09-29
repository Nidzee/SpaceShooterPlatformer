using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrenadeHandler : MonoBehaviour
{
    [Header("Grenade config")]
    [SerializeField] Transform _throwPoint;
    [SerializeField] Grenade _grenadePrefab;

    [Header("Sounds config")]
    [SerializeField] AudioClip _throwSound;

    [Header("Visuals")]
    [SerializeField] Slider _forceSlider;


    [SerializeField] float _grenadeDamagePoints;
    [SerializeField] int _minThrowForce = 3;
    [SerializeField] int _maxThrowForce = 13;
    [SerializeField] float _forceChangeSpeed = 4;

    float _currentForce;
    bool _isActive;
    bool _isIncreasing = true;



    public void StartForceSet()
    {
        _isActive = true;
        _isIncreasing = true;
        _currentForce = _minThrowForce;

        _forceSlider.minValue = _minThrowForce;
        _forceSlider.maxValue = _maxThrowForce;
    }

    public void ReleaseForceSet()
    {
        LaunchGrenade();
        _isActive = false;
        _forceSlider.value = _minThrowForce;
    }

    void Update()
    {
        if (!_isActive)
        {
            return;
        }

        if (_isIncreasing)
        {
            _currentForce += Time.deltaTime * _forceChangeSpeed;
            if (_currentForce >= _maxThrowForce)
            {
                _isIncreasing = false;
                _currentForce = _maxThrowForce;
            }
        } 
        else
        {
            _currentForce -= Time.deltaTime * _forceChangeSpeed;
            if (_currentForce <= _minThrowForce)
            {
                _isIncreasing = true;
                _currentForce = _minThrowForce;
            }
        }

        _forceSlider.value = _currentForce;
    }

    public void LaunchGrenade()
    {
        var grenade = Instantiate(_grenadePrefab, _throwPoint.position, _throwPoint.rotation);
        var grenadeComp = grenade.GetComponent<Grenade>();
        grenadeComp.SetStats(_grenadeDamagePoints);
        grenadeComp.LaunchGrenade(_currentForce);
    
    
        AudioSource.PlayClipAtPoint(_throwSound, transform.position);
    }
}
