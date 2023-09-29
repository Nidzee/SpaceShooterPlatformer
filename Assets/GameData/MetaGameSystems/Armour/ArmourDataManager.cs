using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class ArmourDataManager : MonoBehaviour
{
    // Static reference
    public static ArmourDataManager Instance;
    
    void Awake()
    {
        Instance = this;
    }


    // Manager data
    [SerializeField] ArmourDataConfig _armourDataConfig;
    Dictionary<int, ArmourLevelDataConfig> _armourLevelCache;

    // Copy of actual player data -> used to detect if data was updated -> to refresh UI
    PlayerSaveData_Armour _armourSaveDataCopy;

    // To track if we reach top of config -> to prevent next upgrade
    PlayerSaveData_Armour _topSaveConfig;

    // Event triggered after armour data was modified
    [HideInInspector] public UnityEvent OnDataChanged_Armour;




    public void InitManager()
    {
        BuildManagerCache();


        // Save copy of actual player data
        _armourSaveDataCopy = PlayerDataManager.Instance.PlayerData.ArmourData.GetCopy();


        // If player data was updated -> check if armour data was updated
        PlayerDataManager.Instance.OnDataChanged.AddListener(OnPlayerGameDataChanged);
    }

    void BuildManagerCache()
    {
        // Skip if no data provided
        if (_armourDataConfig == null ||  _armourDataConfig.ArmourLevelsConfigCollection == null ||  _armourDataConfig.ArmourLevelsConfigCollection.Count <= 0)
        {
            Debug.LogError("[Armour-Data-Manager] Data is not provided.");
            return;
        }


        // Clear the cache
        _armourLevelCache = new Dictionary<int, ArmourLevelDataConfig>();

        // Build the cache
        for (int i = 0; i < _armourDataConfig.ArmourLevelsConfigCollection.Count; i++)
        {
            // Check if config is valid
            var config = _armourDataConfig.ArmourLevelsConfigCollection[i];

            config.BuildCache();
            _armourLevelCache[i] = config;
        }


        // Get top config
        var topConfig = _armourLevelCache.LastOrDefault();
        _topSaveConfig = new PlayerSaveData_Armour()
        {
            ArmourLevel = topConfig.Key,
            ArmourLevelStep = (topConfig.Value.GetStepsAmount()-1)
        };
    }

    void OnPlayerGameDataChanged()
    {
        Debug.Log("[Armour-Data-Manager] OnDataChanged triggered.");
        var actualData = PlayerDataManager.Instance.PlayerData.ArmourData;
        bool isDataEqual = actualData.IsEqual(_armourSaveDataCopy);

        if (!isDataEqual)
        {
            Debug.Log("[Armour-Data-Manager] Armour data updated! Event triggered.");
            _armourSaveDataCopy = actualData.GetCopy();
            OnDataChanged_Armour.Invoke();
        }
    }

    public ArmourStepStats GetArmourStepStats(PlayerSaveData_Armour data)
    {
        int level = data.ArmourLevel;
        var test = _armourLevelCache.TryGetValue(level, out var result);
        return result.StepStatsCollection[data.ArmourLevelStep];
    } 




    public bool IsTopConfig(PlayerSaveData_Armour data)
    {
        return data.IsEqual(_topSaveConfig);
    }




   
    public PlayerSaveData_Armour GetUpgradedData()
    {
        int currentLevel = _armourSaveDataCopy.ArmourLevel;
        int currentStepIndex = _armourSaveDataCopy.ArmourLevelStep;
        
        int newLevelNumber = 0;
        int newStepIndex = 0;


        // Skip if data by level is out of bounds
        if (!_armourLevelCache.TryGetValue(currentLevel, out var levelData))
        {
            Debug.LogError("[Armour-Data-Manager] Player-Level is out of config bounds.");
            return null;
        }


        int currentLevelStepsAmount = levelData.GetStepsAmount();
        int currentLevelMaxStepIndex = currentLevelStepsAmount-1;


        int maxLevel = _armourLevelCache.LastOrDefault().Key;
        
        currentStepIndex++;

        
        // If next step is bigger then maxIndex -> move to next level
        if (currentStepIndex > currentLevelMaxStepIndex)
        {
            // Increase level number
            int nextLevel = currentLevel + 1;

            // If we are out of bounds -> save max top value
            if (nextLevel > maxLevel)
            {
                // TODO: Add prevent if we are trying to upgrade top config
                Debug.LogError("Try to increase armour. We are on top value.");
                newLevelNumber = maxLevel;
                newStepIndex = currentLevelMaxStepIndex;
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
            newLevelNumber = currentLevel;
            newStepIndex = currentStepIndex;
        }



        return new PlayerSaveData_Armour()
        {
            ArmourLevel = newLevelNumber,
            ArmourLevelStep = newStepIndex
        };
    }











    public int GetUpgradePrice()
    {
        return 100;
    } 

    public void TryToUpgradeArmour()
    {
        Debug.Log("[Armour-Data-Manager] Try to upgrade armour...");
        PlayerDataManager.Instance.TryToUpgradeArmour(GetUpgradePrice());
    }










    public PlayerSaveData_Armour ShrinkToConfigBounds(PlayerSaveData_Armour armourData)
    {
        // Check if cache was build
        if (_armourLevelCache == null || _armourLevelCache.Count <= 0)
        {
            Debug.LogError("[Armour-Data-Manager] Cannot shrink data. No cache builded.");
            return new PlayerSaveData_Armour();
        }



        int playerLevel = armourData.ArmourLevel;
        int playerSteps = armourData.ArmourLevelStep;


        // Check if saved-armour-level is in config
        // If not -> shrink to fit in config
        if (!_armourLevelCache.ContainsKey(playerLevel))
        {
            Debug.Log("[Armour-Data-Manager] [1] Shrink the level number.");
            
            int topLevelNumber = _armourLevelCache.LastOrDefault().Key;

            if (playerLevel < 0)
            {
                Debug.Log("[Armour-Data-Manager] [1] Level lower than 0. Set as 0.");
                playerLevel = 0;
            }
            else if (playerLevel > topLevelNumber)
            {
                Debug.Log("[Armour-Data-Manager] [1] Level greater than top. Set as top.");
                playerLevel = topLevelNumber;
            } 
            else
            {
                Debug.LogError("[Armour-Data-Manager] Impossible bug. Level number must be inside bounds.");
            }
        }


        // After level shrinkin -> we can take data by level from config
        // Gect player level config data
        var levelData = _armourLevelCache[playerLevel];


        int playerLevelConfigStepsAmount = levelData.GetStepsAmount();
        int topIndex = playerLevelConfigStepsAmount-1;


        // Check if steps index is in bounds
        if (playerSteps < 0 || playerSteps > topIndex)
        {
            Debug.Log("[Armour-Data-Manager] [1] Shrink the step number.");

            if (playerSteps < 0)
            {
                Debug.Log("[Armour-Data-Manager] [1] Step lower than 0. Set as 0.");
                playerSteps = 0;
            }
            else if (playerSteps > topIndex)
            {
                Debug.Log("[Armour-Data-Manager] [1] Step greater than top. Set as top.");
                playerSteps = topIndex;
            } 
            else
            {
                Debug.LogError("[Armour-Data-Manager] Impossible bug. Step number must be inside bounds.");
            }
        }


        // Return shrink data
        return new PlayerSaveData_Armour()
        {
            ArmourLevel = playerLevel,
            ArmourLevelStep = playerSteps,
        };
    }
}