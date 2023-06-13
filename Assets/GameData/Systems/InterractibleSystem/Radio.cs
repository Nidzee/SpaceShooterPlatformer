using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Radio : MonoBehaviour, IInteractible
{
    [SerializeField] AudioSource _audioSource;
    [SerializeField] AudioClip _song;
    bool _isRadioActive;

    public void Interact()
    {
        if (_isRadioActive)
        {
            _isRadioActive = false;
            _audioSource.Stop();
            return;
        }

        _isRadioActive = true;
        _audioSource.clip = _song;
        _audioSource.Play();
    }
}