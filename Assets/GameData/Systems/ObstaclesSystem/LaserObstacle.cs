using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public enum LaserObstacleType
{
    Infinite,
    FireSleep,
}

public class LaserObstacle : MonoBehaviour
{
    [SerializeField] LaserObstacleType _laserType;

    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _startPoint;
    [SerializeField] float _maxRayDistance;

    [Header("Visuals")]
    [SerializeField] Transform _startLaser;
    [SerializeField] Transform _finishLaser;

    [SerializeField] float _fireDuration;
    [SerializeField] float _sleepDuration;
    float _sleepTimer;
    float _fireTimer;



    public void Update()
    {
        if (_laserType == LaserObstacleType.Infinite)
        {
            LaunchRay();
            return;
        }


        if (_sleepTimer > 0)
        {
            _sleepTimer -=Time.deltaTime;
            return;
        }


        if (_fireTimer > 0)
        {
            LaunchRay();
            _fireTimer -= Time.deltaTime;
        }
        else
        {
            DeactivateLaser();
            _sleepTimer = _sleepDuration;
            _fireTimer = _fireDuration;
        }
    }

    void LaunchRay()
    {

        RaycastHit2D hit = Physics2D.Raycast(_startPoint.position, _startPoint.right, _maxRayDistance);

        if (hit.collider != null)
        {
            _startLaser.gameObject.SetActive(true);
            _finishLaser.gameObject.SetActive(true);

            _startLaser.transform.position = _startPoint.transform.position;
            _finishLaser.transform.position = hit.point;

            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, _startPoint.position);
            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            DeactivateLaser();
        }
    }

    void DeactivateLaser()
    {
        _lineRenderer.enabled = false;
        _startLaser.gameObject.SetActive(false);
        _finishLaser.gameObject.SetActive(false);
    }
}