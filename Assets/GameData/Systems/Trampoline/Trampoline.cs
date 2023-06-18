using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] float _fallMinForceForTrigger;
    [SerializeField] float _launchForce;




    
    void OnTriggerEnter2D(Collider2D collider) {

        Rigidbody2D rigidbody = collider.gameObject.GetComponent<Rigidbody2D>();
        if (rigidbody == null)
        {
            return;
        }

        float yVelocityModule = Mathf.Abs(rigidbody.velocity.y);
        if (yVelocityModule >= _fallMinForceForTrigger)
        {
            rigidbody.velocity = new Vector2(rigidbody.velocity.x, 0);
            Debug.Log("Trigger trampoline");
            Vector2 launchForce = transform.up * _launchForce;
            rigidbody.AddForce(launchForce, ForceMode2D.Impulse);
        }
    }
}