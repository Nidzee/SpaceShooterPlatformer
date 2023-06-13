using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionHandler : MonoBehaviour
{
    [SerializeField] PlayerController _player;
    [SerializeField] GameObject _interactionAvailbaleObject;

    IInteractible _currentInteractible;


    public void Reset()
    {
        _currentInteractible = null;
        _interactionAvailbaleObject.SetActive(false);
    }

    public void TryToInteract()
    {
        // Skip if no interactible around
        if (_currentInteractible == null)
        {
            Debug.Log("Try to interact -> no interactibles around.");
            return;
        }


        _currentInteractible.Interact();
    }






    public void RegisterInteractible(IInteractible unit)
    {
        Debug.Log("[Interactor] Register interaction");
        // Activate Icon
        _interactionAvailbaleObject.SetActive(true);

        // Save current interactible unit reference
        _currentInteractible = unit;
    }

    public void UnregisterInteractible(IInteractible unit)
    {
        Debug.Log("[Interactor] Unregister interaction");

        // Remove Icon
        _interactionAvailbaleObject.SetActive(false);

        // Save current interactible unit reference
        _currentInteractible = null;
    }
}