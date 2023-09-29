using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DoorOpenTriggerZone : MonoBehaviour
{
    public UnityEvent OnTriggerEnter;
    public UnityEvent OnTriggerExit;


    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == TagConstraintsConfig.PLAYER_TAG)
        {
            OnTriggerEnter.Invoke();
        }
    }
    
    public void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == TagConstraintsConfig.PLAYER_TAG)
        {
            OnTriggerExit.Invoke();
        }
    } 
}