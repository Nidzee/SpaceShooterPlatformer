using System.Collections;
using System.Collections.Generic;
using UnityEngine;



// Data for each step of progress
[System.Serializable]
public class ArmourStepStats
{
    public int ArmourValue;
}



// Data for armour-level
// Defines amount of steps in this level
[System.Serializable]
public class ArmourLevelDataConfig
{
    // List of steps for level
    public List<ArmourStepStats> StepStatsCollection;
    
    // Cached data
    Dictionary<int, ArmourStepStats> _armourStepStatsCache;
    int _stepsAmount;


    public void BuildCache()
    {
        // Steps in this level
        _stepsAmount = StepStatsCollection.Count;


        // Build cache for steps->values
        _armourStepStatsCache = new Dictionary<int, ArmourStepStats>();
        for (int i = 0; i < StepStatsCollection.Count; i++)
        {
            ArmourStepStats item = StepStatsCollection[i];
            _armourStepStatsCache[i] = item;
        }
    }

    public int GetStepsAmount()
    {
        return _stepsAmount;
    }
}



// Conrainer class for all armour data-config
[System.Serializable]
public class ArmourDataConfig
{
    public List<ArmourLevelDataConfig> ArmourLevelsConfigCollection;
}