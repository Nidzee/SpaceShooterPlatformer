using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class HealthDataManager : Manager<HealthDataManager>
{
    PlayerSaveData_Health _healthSaveDataCopy;
    [HideInInspector] public UnityEvent OnDataChanged_Health = new UnityEvent();





    public override void init()
    {
        _healthSaveDataCopy = PlayerDataManager.Instance.PlayerData.HealthData.GetCopy();
        PlayerDataManager.Instance.OnDataChanged.AddListener(OnPlayerGameDataChanged);
    }

    void OnPlayerGameDataChanged()
    {
        var actualData = PlayerDataManager.Instance.PlayerData.HealthData;
        bool isDataEqual = actualData.IsEqual(_healthSaveDataCopy);

        if (!isDataEqual)
        {
            _healthSaveDataCopy = actualData.GetCopy();
            OnDataChanged_Health.Invoke();
        }
    }






    public PlayerSaveData_Health GetActualData()
    {
        return _healthSaveDataCopy;
    }

    public HealthStepStats GetHealthStepStats(PlayerSaveData_Health data)
    {
        int level = data.HelathLevel;
        int step = data.HelathLevelStep;


        int topConfigLevel = HealthSystemDataConfig.HealthLevelsConfigCollection.Count - 1;
        if (level > topConfigLevel)
        {
            Debug.LogWarning("[HEA] Warning try to get data for level outside of config bounds (LEVEL)");
            return null;
        }



        var levelData = HealthSystemDataConfig.HealthLevelsConfigCollection[level];
        int topStepNumber = levelData.StepStatsCollection.Count - 1;
        if (step > topStepNumber)
        {
            Debug.LogWarning("[HEA] Warning try to get data for level outside of config bounds (STEP)");
            return null;
        }


        return levelData.StepStatsCollection[step];
    } 







   
    public PlayerSaveData_Health GetUpgradedData()
    {
        int currentLevelIndex = _healthSaveDataCopy.HelathLevel;
        int currentStepIndex = _healthSaveDataCopy.HelathLevelStep;
        

        // Check if current level is not outside of config
        int topConfigLevelIndex = HealthSystemDataConfig.HealthLevelsConfigCollection.Count - 1;
        if (currentLevelIndex > topConfigLevelIndex)
        {
            return null;
        }


        // Get current player level data
        var currentLevelData = HealthSystemDataConfig.HealthLevelsConfigCollection[currentLevelIndex];


        // Check if current step number is not outside of current player level data
        int topStepIndexForCurrentLevel = currentLevelData.StepStatsCollection.Count - 1;
        if (currentStepIndex > topStepIndexForCurrentLevel)
        {
            return null;
        }





        // Increment step and get upgraded data
        currentStepIndex++;
        int newLevelNumber = 0;
        int newStepIndex = 0;


        // If next step is bigger then maxIndex -> move to next level
        if (currentStepIndex > topStepIndexForCurrentLevel)
        {
            // Increase level number
            int nextLevel = currentLevelIndex + 1;

            // If we are out of bounds -> save max top value
            if (nextLevel > topConfigLevelIndex)
            {
                // TODO: Add prevent if we are trying to upgrade top config
                Debug.LogError("Try to increase health. We are on top value.");
                newLevelNumber = topConfigLevelIndex;
                newStepIndex = topStepIndexForCurrentLevel;
            }
            else
            {
                // Move to next level with step 0
                newLevelNumber = nextLevel;
                newStepIndex = 0;
            }
        }
        else
        {
            // Same level, increase step
            newLevelNumber = currentLevelIndex;
            newStepIndex = currentStepIndex;
        }







        return new PlayerSaveData_Health()
        {
            HelathLevel = newLevelNumber,
            HelathLevelStep = newStepIndex
        };
    }










    public int GetUpgradePrice()
    {
        var currentData = GetHealthStepStats(_healthSaveDataCopy);
        if (currentData == null)
        {
            return -1;
        }

        return currentData.UpgradeCost;
    } 

    public void TryToUpgradeHealth()
    {
        PlayerDataManager.Instance.TryToUpgradeHealth(GetUpgradePrice());
    }
}












public static class HealthSystemDataConfig 
{
    public static List<HealthLevelDataConfig> HealthLevelsConfigCollection = new List<HealthLevelDataConfig>()
    {

        // Level: 0
        new HealthLevelDataConfig() 
        {
            StepStatsCollection = new List<HealthStepStats>()
            {
                new HealthStepStats()
                {
                    UpgradeCost = 15,
                    HealthValue = 10,
                },
                new HealthStepStats()
                {
                    UpgradeCost = 20,
                    HealthValue = 11
                },
                new HealthStepStats()
                {
                    UpgradeCost = 25,
                    HealthValue = 12
                },
                new HealthStepStats()
                {
                    UpgradeCost = 30,
                    HealthValue = 13
                },
            }
        },
    };
}




public class HealthLevelDataConfig
{
    public List<HealthStepStats> StepStatsCollection;
}

public class HealthStepStats
{
    public int UpgradeCost;
    public int HealthValue;
}