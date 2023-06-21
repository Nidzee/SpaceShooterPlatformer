using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LaserObstacleNew : MonoBehaviour
{
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _startPoint;
    [SerializeField] float _maxRayDistance;

    [Header("Visuals")]
    [SerializeField] Transform _startLaser;
    [SerializeField] Transform _finishLaser;





    public void Update()
    {
        LaunchRay();
    }

    void LaunchRay()
    {

        RaycastHit2D hit = Physics2D.Raycast(_startPoint.position, _startPoint.right, _maxRayDistance);
        _startLaser.transform.position = _startPoint.transform.position;

        if (hit.collider != null)
        {
            _lineRenderer.enabled = true;
            _finishLaser.transform.position = hit.point;

            _lineRenderer.SetPosition(0, _startPoint.position);
            _lineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            _lineRenderer.enabled = false;
        }
    }
}