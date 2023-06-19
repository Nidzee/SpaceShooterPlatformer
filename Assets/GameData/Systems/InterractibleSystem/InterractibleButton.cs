using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InterractibleButton : MonoBehaviour, IInteractible
{
    [SerializeField] InterractibleButtonType _buttonType;
    [SerializeField] AudioClip _interactSound_Success;
    [SerializeField] AudioClip _interactSound_Fail;

    [SerializeField] float _interractCoolDown;
    
    float _currentCoolDown = 0;
    bool _isActive = false;
    bool _isCompleted = false;

    public UnityEvent OnButtonPressed;


    void Start()
    {
        _isActive = false;
        _isCompleted = false;
    }


    public void Interact()
    {
        if (_buttonType == InterractibleButtonType.OnlyOn)
        {
            if (_isCompleted)
            {
                Debug.Log("Button was already pressed.");
                return;
            } 
            else
            {
                AudioSource.PlayClipAtPoint(_interactSound_Success, transform.position);
                OnButtonPressed.Invoke();
                _isCompleted = true;
            }

        }
        else if (_buttonType == InterractibleButtonType.OnOff)
        {
            if (_currentCoolDown >= 0)
            {
                Debug.Log("COOL DOWN IS NOT FINISHED");
                AudioSource.PlayClipAtPoint(_interactSound_Fail, transform.position);
                return;
            }


            _isActive = true;
            _currentCoolDown = _interractCoolDown;
            AudioSource.PlayClipAtPoint(_interactSound_Success, transform.position);
            OnButtonPressed.Invoke();
        }
    }


    void Update()
    {
        if (_currentCoolDown >= 0)
        {
            _currentCoolDown -= Time.deltaTime;
        }
    }
}

public enum InterractibleButtonType
{
    OnlyOn,
    OnOff,
}