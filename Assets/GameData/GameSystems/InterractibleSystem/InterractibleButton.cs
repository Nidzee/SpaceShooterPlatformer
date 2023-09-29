using UnityEngine.Events;
using UnityEngine;

public class InterractibleButton : MonoBehaviour, IInteractible
{
    [Header("Button config")]
    [SerializeField] InterractibleButtonType _buttonType;
    [SerializeField] float _interractCoolDownSeconds;
    
    [Header("Sounds config")]
    [SerializeField] AudioClip _interactSound_Success;
    [SerializeField] AudioClip _interactSound_Fail;

    // Button press event
    [HideInInspector] public UnityEvent OnButtonPressed;
    
    // Data for on-off button-type
    float _currentCoolDown = 0;

    // Data for only-on button-type
    bool _isButtonPressed = false;




    void Start()
    {
        _isButtonPressed = false;
        _currentCoolDown = 0;
    }


    public void Interact()
    {
        if (_buttonType == InterractibleButtonType.OnlyOn)
        {
            HandleOnlyOnButtonLogic();
            return;
        }
        
        if (_buttonType == InterractibleButtonType.OnOff)
        {
            HandleOnOffButtonLogic();
            return;
        }
    }

    void HandleOnlyOnButtonLogic()
    {
        // Skip if button was already pressed
        if (_isButtonPressed)
        {
            Debug.Log("Button was already pressed.");
            return;
        } 
        

        _isButtonPressed = true;
        AudioSource.PlayClipAtPoint(_interactSound_Success, transform.position);
        OnButtonPressed.Invoke();
    }

    void HandleOnOffButtonLogic()
    {
        // Wait for cool down to trigger click
        if (_currentCoolDown >= 0)
        {
            AudioSource.PlayClipAtPoint(_interactSound_Fail, transform.position);
            return;
        }


        _currentCoolDown = _interractCoolDownSeconds;
        AudioSource.PlayClipAtPoint(_interactSound_Success, transform.position);
        OnButtonPressed.Invoke();
    }



    // CoolDown timer logic
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
    OnlyOn = 0,
    OnOff = 1,
}