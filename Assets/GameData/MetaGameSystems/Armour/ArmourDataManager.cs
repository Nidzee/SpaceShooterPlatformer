using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class ArmourDataManager : Manager<ArmourDataManager>
{
    PlayerSaveData_Armour _armourSaveDataCopy;

    public UnityEvent OnDataChanged_Armour = new UnityEvent();






    public override void init()
    {
        _armourSaveDataCopy = PlayerDataManager.Instance.PlayerData.ArmourData.GetCopy();
        PlayerDataManager.Instance.OnDataChanged.AddListener(OnPlayerGameDataChanged);
    }

    void OnPlayerGameDataChanged()
    {
        var actualData = PlayerDataManager.Instance.PlayerData.ArmourData;
        bool isDataEqual = actualData.IsEqual(_armourSaveDataCopy);

        if (!isDataEqual)
        {
            _armourSaveDataCopy = actualData.GetCopy();
            OnDataChanged_Armour.Invoke();
        }
    }





    public PlayerSaveData_Armour GetActualData()
    {
        return _armourSaveDataCopy;
    }

    public ArmourStepStats GetArmourStepStats(PlayerSaveData_Armour data)
    {
        int level = data.ArmourLevel;
        int step = data.ArmourLevelStep;


        int topConfigLevel = ArmourSystemDataConfig.ArmourLevelsConfigCollection.Count - 1;
        if (level > topConfigLevel)
        {
            Debug.LogWarning("[ARM] Warning try to get data for level outside of config bounds (LEVEL)");
            return null;
        }



        var levelData = ArmourSystemDataConfig.ArmourLevelsConfigCollection[level];
        int topStepNumber = levelData.StepStatsCollection.Count - 1;
        if (step > topStepNumber)
        {
            Debug.LogWarning("[ARM] Warning try to get data for level outside of config bounds (STEP)");
            return null;
        }


        return levelData.StepStatsCollection[step];
    } 







   
    public PlayerSaveData_Armour GetUpgradedData()
    {
        int currentLevelIndex = _armourSaveDataCopy.ArmourLevel;
        int currentStepIndex = _armourSaveDataCopy.ArmourLevelStep;
        

        // Check if current level is not outside of config
        int topConfigLevelIndex = ArmourSystemDataConfig.ArmourLevelsConfigCollection.Count - 1;
        if (currentLevelIndex > topConfigLevelIndex)
        {
            return null;
        }


        // Get current player level data
        var currentLevelData = ArmourSystemDataConfig.ArmourLevelsConfigCollection[currentLevelIndex];


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
                Debug.LogError("Try to increase armour. We are on top value.");
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







        return new PlayerSaveData_Armour()
        {
            ArmourLevel = newLevelNumber,
            ArmourLevelStep = newStepIndex
        };
    }







    public bool IsTopConfig(PlayerSaveData_Armour config)
    {
        int topConfigLevel = ArmourSystemDataConfig.ArmourLevelsConfigCollection.Count - 1;
        if (config.ArmourLevel < topConfigLevel)
        {
            return false;
        }


        var levelData = ArmourSystemDataConfig.ArmourLevelsConfigCollection[config.ArmourLevel];
        int topStepIndex = levelData.StepStatsCollection.Count - 1;
        if (config.ArmourLevelStep < topStepIndex)
        {
            return false;
        }


        return true;
    }





    public int GetUpgradePrice()
    {
        var currentData = GetArmourStepStats(_armourSaveDataCopy);
        if (currentData == null)
        {
            return -1;
        }

        return currentData.UpgradeCost;
    } 

    public void TryToUpgradeArmour()
    {
        PlayerDataManager.Instance.TryToUpgradeArmour(GetUpgradePrice());
    }
}








public static class ArmourSystemDataConfig 
{
    public static List<ArmourLevelDataConfig> ArmourLevelsConfigCollection = new List<ArmourLevelDataConfig>()
    {

        // Level: 0
        new ArmourLevelDataConfig() 
        {
            StepStatsCollection = new List<ArmourStepStats>()
            {
                new ArmourStepStats()
                {
                    UpgradeCost = 15,
                    ArmourValue = 10
                },
                new ArmourStepStats()
                {
                    UpgradeCost = 20,
                    ArmourValue = 11
                },
                new ArmourStepStats()
                {
                    UpgradeCost = 25,
                    ArmourValue = 12
                },
                new ArmourStepStats()
                {
                    UpgradeCost = 30,
                    ArmourValue = 13
                },
            }
        },



        // Level: 1
        new ArmourLevelDataConfig() 
        {
            StepStatsCollection = new List<ArmourStepStats>()
            {
                new ArmourStepStats()
                {
                    UpgradeCost = 100,
                    ArmourValue = 15
                },
                new ArmourStepStats()
                {
                    UpgradeCost = 110,
                    ArmourValue = 20
                },
                new ArmourStepStats()
                {
                    UpgradeCost = 120,
                    ArmourValue = 25
                },
                new ArmourStepStats()
                {
                    UpgradeCost = 130,
                    ArmourValue = 30
                },
            }
        },
    };
}




public class ArmourLevelDataConfig
{
    public List<ArmourStepStats> StepStatsCollection;
}

public class ArmourStepStats
{
    public int UpgradeCost;
    public int ArmourValue;
}