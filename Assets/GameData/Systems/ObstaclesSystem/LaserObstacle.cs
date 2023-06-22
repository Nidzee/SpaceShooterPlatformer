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
    [Header("Laser obstacle config")]
    [SerializeField] LaserObstacleType _laserType;
    [SerializeField] LineRenderer _lineRenderer;
    [SerializeField] Transform _startPoint;
    [SerializeField] float _maxRayDistance;
    [SerializeField] LayerMask _lazerHitMask; // Defines layers that laser can hit and place line redere
    [SerializeField] GameObject _laserCollider;

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

        RaycastHit2D hit = Physics2D.Raycast(_startPoint.position, _startPoint.right, _maxRayDistance, _lazerHitMask);

        if (hit.collider != null)
        {
            _startLaser.gameObject.SetActive(true);
            _finishLaser.gameObject.SetActive(true);
            _laserCollider.gameObject.SetActive(true);

            _startLaser.transform.position = _startPoint.transform.position;
            _finishLaser.transform.position = hit.point;


            Vector3 middle = Vector3.Lerp(_startLaser.transform.position, _finishLaser.transform.position, 0.5f);
            _laserCollider.transform.position = middle;
            _laserCollider.transform.up = _finishLaser.transform.position - _startLaser.transform.position;

            float distance = Vector3.Distance(_startLaser.transform.position, _finishLaser.transform.position);
            _laserCollider.transform.localScale = new Vector3(1, distance, 1);


            _lineRenderer.enabled = true;
            _lineRenderer.SetPosition(0, _startPoint.position);
            _lineRenderer.SetPosition(1, hit.point);
            GenerateMeshCollider();
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
        _laserCollider.gameObject.SetActive(false);
    }


    public void GenerateMeshCollider()
    {
        MeshCollider collider = GetComponent<MeshCollider>();

        if (collider == null)
        {
            collider = gameObject.AddComponent<MeshCollider>();
        }


        Mesh mesh = new Mesh();
        _lineRenderer.BakeMesh(mesh, true);

        // if you need collisions on both sides of the line, simply duplicate & flip facing the other direction!
        // This can be optimized to improve performance ;)
        int[] meshIndices = mesh.GetIndices(0);
        int[] newIndices = new int[meshIndices.Length * 2];

        int j = meshIndices.Length - 1;
        for (int i = 0; i < meshIndices.Length; i++)
        {
            newIndices[i] = meshIndices[i];
            newIndices[meshIndices.Length + i] = meshIndices[j];
        }
        mesh.SetIndices(newIndices, MeshTopology.Triangles, 0);

        collider.sharedMesh = mesh;
    }

    
    private bool IsRayCollideSurface(GameObject collidedObject, LayerMask mask)
    {
        return ((1 << collidedObject.gameObject.layer) & mask) != 0;
    }
}