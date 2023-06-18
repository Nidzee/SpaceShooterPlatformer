using System.Threading.Tasks;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _lifeTime = 3f;
    [SerializeField] float _bulletSpeed;
    [SerializeField] Rigidbody2D _rb;
    
    float _damagePoints;



    public void SetStats(float damagePoints)
    {
        _damagePoints = damagePoints;
    }

    public void LaunchBullet()
    {
        Vector3 bulletDirection = transform.right;

        _rb.velocity = bulletDirection * _bulletSpeed;
        
        DelayDestruction();
    }



    void OnCollisionEnter2D(Collision2D collision) {

        // Get collider tag of object we reach
        string colliderTag = collision.gameObject.tag;

        switch (colliderTag)
        {

            case TagConstraintsConfig.DESTRUCTIBLE_UNIT_TAG:
            var data = collision.gameObject.GetComponent<IDamageble>();
            if (data != null)
            {
                data.TakeDamage(_damagePoints);
            }
            DestroyBullet();
            break;


            default:
            DestroyBullet();
            break;
        }
    }

    public void DestroyBullet()
    {
        Destroy(gameObject);
    }

    async void DelayDestruction()
    {
        int delayBeforeDestruction = (int)_lifeTime * 1000;
        await Task.Delay(delayBeforeDestruction);

        if (this != null)
        {
            DestroyBullet();
        }
    }
}