using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class MovingPlatformController : MonoBehaviour
{
    [Header("Type Configuration")]
    [SerializeField] VisitPointsType _visitType;

    [Header("Data Configuration")]
    [SerializeField] List<Transform> _wayPointsList;    // Collection of way points
    [SerializeField] float _sleepDelay;                 // Delay stay on point
    [SerializeField] float _moveDuration;               // Duration from point - to - point

    // Private data
    int _currentTargetIndex;
    Transform _target;



    void Start()
    {
        if (_wayPointsList == null || _wayPointsList.Count < 2)
        {
            Debug.LogError("Error! Moving platform can not move. No points set.");
            return;
        }

        StartPlatformController();
    }

    async void StartPlatformController()
    {
        _currentTargetIndex = 0;
        _target = _wayPointsList[_currentTargetIndex];

        while(true)
        {
            await LaunchPlatformLogic();
        }
    }



    async Task LaunchPlatformLogic()
    {
        // Move from point to point
        await LerpPositionFixedTime();

        // If delay required -> delay each point sleep
        if (_sleepDelay > 0)
        {
            await DelaySleepTime();
        }

        // Select next point by visit-type
        SelectNextPoint();
    }

    void SelectNextPoint()
    {
        // Move to next point
        _currentTargetIndex++;

        // Specify next target
        if (_currentTargetIndex >= _wayPointsList.Count)
        {
            if (_visitType == VisitPointsType.ReachTopPointMoveToStart)
            {
                _currentTargetIndex = 0;
            }
            else
            {
                // We are already on 0th point -> next target is 0++
                _currentTargetIndex = 1;
                _wayPointsList.Reverse();
            }
        }
        
        // Set next target point
        _target = _wayPointsList[_currentTargetIndex];
    }




    private async Task DelaySleepTime()
    {
        int miliseconds = (int)_sleepDelay * 1000;
        await Task.Delay(miliseconds);
    }

    private async Task LerpPositionFixedTime()
    {
        Vector3 fromPosition = transform.position;
        Vector3 toPosition = _target.position;

        float step = 0;         // from 0 -> 1
        float timeElapsed = 0;  // time elapsed since start

        while (timeElapsed < _moveDuration)
        {
            timeElapsed += Time.deltaTime;
            step = timeElapsed / _moveDuration;
            transform.position = Vector3.Lerp(fromPosition,toPosition, step);
            await Task.Yield();
        }
    }



    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag.Equals(TagConstraintsConfig.PLAYER_TAG))
        {
            col.transform.parent = this.transform;
        }
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.tag.Equals(TagConstraintsConfig.PLAYER_TAG))
        {
            col.transform.parent = null;
        }
    }
}

public enum VisitPointsType
{
    EachPoint = 0,
    ReachTopPointMoveToStart = 1,
}