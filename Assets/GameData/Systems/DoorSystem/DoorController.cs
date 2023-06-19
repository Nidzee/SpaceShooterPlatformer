using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] Transform _doorObject;
    [SerializeField] Transform _dorOpenedPos;
    [SerializeField] Transform _doorClosePos;
    [SerializeField] float _moveSpeed;
    [SerializeField] DoorType _doorType;
    [SerializeField] List<InterractibleButton> _buttonTriggersCollection;

    [SerializeField] DoorOpenTriggerZone _triggerZone;

    bool _isActive = false;
    Vector3 _fromPosition;
    Vector3 _toPosition;


    public void Start()
    {
        _isActive = false;

        if (_doorType == DoorType.OpenTriggerZone)
        {
            if (_triggerZone != null)
            {
                _triggerZone.OnTriggerEnter.AddListener(OpenDoor);
                _triggerZone.OnTriggerExit.AddListener(CloseDoor);
            } 
            else
            {
                Debug.LogError("Error! Door is with trigger zone type and missing trigger-zone reference");
            }
        }
        else if (_doorType == DoorType.ButtonOpen)
        {
            if (_buttonTriggersCollection == null || _buttonTriggersCollection.Count <= 0)
            {
                Debug.LogError("Error! Door is with trigger button type and missing button reference");
            }
            else
            {
                foreach (var btn in _buttonTriggersCollection)
                {
                    btn.OnButtonPressed.AddListener(TriggerDoorLogic);
                }
            }
        }
    }

    void TriggerDoorLogic()
    {
        // Dor is in start position
        if (_toPosition == Vector3.zero)
        {
            OpenDoor();
            return;
        }

        // Door is closing or closed
        if (_toPosition == _doorClosePos.transform.position)
        {
            OpenDoor();
            return;
        }

        // Door is closing or closed
        if (_toPosition == _dorOpenedPos.transform.position)
        {
            CloseDoor();
            return;
        }
    }

    void OpenDoor()
    {
        _isActive = true;
        _fromPosition = _doorObject.transform.position;
        _toPosition = _dorOpenedPos.transform.position;
    }

    void CloseDoor()
    {
        _isActive = true;
        _fromPosition = _doorObject.transform.position;
        _toPosition = _doorClosePos.transform.position;
    }


    void Update()
    {
        if (!_isActive)
        {
            return;
        }

        if (_doorObject.position == _toPosition)
        {
            Debug.Log("REACHED");
            _isActive = false;
        }

        _doorObject.position = Vector3.MoveTowards(_doorObject.position, _toPosition, Time.deltaTime * _moveSpeed);
    }
}

public enum DoorType
{
    OpenTriggerZone = 0,
    ButtonOpen = 1,
    ButtonOpenClose = 2,
}
