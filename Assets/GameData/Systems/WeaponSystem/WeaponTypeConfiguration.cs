using System.Collections.Generic;
using UnityEngine;



// Class holds:
// WEAPON TYPE
// WEAPON DATA

[System.Serializable]
public class WeaponTypeConfiguration 
{
    public WeaponType weaponType;
    public WeaponGameData weaponGameData;
 
    public bool IsConfigValid()
    {
        if (weaponGameData.IsConfigValid())
        {
            return true;
        }

        return false;
    }

    public void BuildCache()
    {
        weaponGameData.BuildCache();
    }
}





// Class holds:
// COLLECTION OF LEVELS FOR WEAPON

[System.Serializable]
public class WeaponGameData
{
    public List<LevelAndStepsConfiguration> weaponLevelConfiguration;
    Dictionary<int, LevelAndStepsConfiguration> _levelStatsDictionary = new Dictionary<int, LevelAndStepsConfiguration>();


    public bool IsConfigValid()
    {
        if (weaponLevelConfiguration?.Count > 0)
        {
            return true;
        }

        return false;
    }


    public void BuildCache()
    {
        foreach (var levelConfig in weaponLevelConfiguration)
        {
            if (!levelConfig.IsConfigValid())
            {
                Debug.LogException(new System.Exception("[weaponLevelConfiguration] Config is invalid."));
                continue;
            }

            levelConfig.BuildCache();
            _levelStatsDictionary[levelConfig.levelNumber] = levelConfig;
        }
    }



    public WeaponStats GetWeaponStats(int levelNumber, int stepNumber)
    {
        // Skip if level not provided
        if (!_levelStatsDictionary.ContainsKey(levelNumber))
        {
            Debug.LogException(new System.Exception("No weapon stst by level in cache."));
            return null;
        }

        LevelAndStepsConfiguration levelConfig = _levelStatsDictionary[levelNumber];

       return levelConfig.GetWeaponStatsByStep(stepNumber);
    }
}





[System.Serializable]
public class LevelAndStepsConfiguration 
{
    // Level number
    public int levelNumber;

    // Collection of steps
    public List<WeaponStats> levelStepsConfiguration;

    // Cahce for quick use
    Dictionary<int, WeaponStats> _stepStatsData = new Dictionary<int, WeaponStats>();


    public bool IsConfigValid()
    {
        if (levelNumber != 0 && levelStepsConfiguration?.Count > 0)
        {
            return true;
        }

        return false;
    }

    public void BuildCache()
    {
        _stepStatsData = new Dictionary<int, WeaponStats>();

        for (int i = 0; i < levelStepsConfiguration.Count; i++)
        {
            int stepNumber = i+1;
            _stepStatsData[stepNumber] = levelStepsConfiguration[i];
        }
    }



    public WeaponStats GetWeaponStatsByStep(int stepNumber)
    {
        if (!_stepStatsData.ContainsKey(stepNumber))
        {
            Debug.LogException(new System.Exception("No stats by step-number in cache."));
            return null;
        }
        
        return _stepStatsData[stepNumber];
    }
}