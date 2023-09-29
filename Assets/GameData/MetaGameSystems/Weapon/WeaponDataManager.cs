using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponDataManager : Manager<WeaponDataManager>
{
    Dictionary<WeaponType, WeaponType_GameData> _weaponTypeDameData = new Dictionary<WeaponType, WeaponType_GameData>();


    PlayerSaveData_Weapon _weaponSaveDataCopy;
    public UnityEvent OnDataChanged_Weapon = new UnityEvent();
    

    // If player data was updated -> check if armour data was updated










    public override void init()
    {
        BuildConfigCache();

        // Save copy of actual player data
        _weaponSaveDataCopy = PlayerDataManager.Instance.PlayerData.WeaponData.GetCopy();
        PlayerDataManager.Instance.OnDataChanged.AddListener(OnPlayerGameDataChanged);
    }

    void OnPlayerGameDataChanged()
    {
        Debug.Log("[Weapon-Data-Manager] OnDataChanged triggered.");
        var actualData = PlayerDataManager.Instance.PlayerData.WeaponData;
        bool isDataEqual = actualData.IsEqual(_weaponSaveDataCopy);

        if (!isDataEqual)
        {
            Debug.Log("[Weapon-Data-Manager] Weapon data updated! Event triggered.");
            _weaponSaveDataCopy = actualData.GetCopy();
            OnDataChanged_Weapon.Invoke();
        }
    }

    





    // Build cache for data-easy-use
    void BuildConfigCache()
    {

        // Build cache [WEAPON TYPE -> WEAPON DATA]
        _weaponTypeDameData = new Dictionary<WeaponType, WeaponType_GameData>();
        foreach (var data in WeaponSystemDataConfig.WeaponData)
        {
            _weaponTypeDameData.Add(data.WeaponType, data);
        }


    }

    public List<WeaponType> GetAllWeaponTypes()
    {
        return _weaponTypeDameData.Keys.ToList();
    }

    public WeaponType_GameData GetWeaponType_GameData(WeaponType type)
    {
        // Try to get data from dictionary
        if (_weaponTypeDameData.TryGetValue(type, out var result))
        {
            return result;
        } 
        else
        {
            Debug.LogError("[###] WEAPON DATA MANAGER - Try to get data by type: " + type + ". No data provided.");
            return null;
        }
    }








    public void UnlockWeponType(WeaponType type)
    {
        // Check if this weapon type is already unlocked
        if (_weaponSaveDataCopy.WeaponsSavesCollection.Any(i => i.WeaponType == type))
        {
            Debug.LogWarning("[###] WARNING TRY TO UNLOCK ALREADY UNLOCKED WEAPON");
            return;
        }


        // Check if we have enough coins
        var data = GetWeaponType_GameData(type);
        int unlockPrice = data.UnlockPrice;
        if (!CurrencyDataManager.Instance.IsEnoughCoins(unlockPrice))
        {
            Debug.LogWarning("[###] NOT ENOUGH COINS");
            return;
        }



        // Unlock this wepon
        PlayerDataManager.Instance.UnlockWeapon(type, unlockPrice);
    }



    Weapon_LevelsConfig GetWeapon_LevelsConfig(WeaponType_GameData data, int levelNumber)
    {
        for (int i = 0; i < data.WeaponLevelConfiguration.Count; i++)
        {
            if (i == levelNumber)
            {
                return data.WeaponLevelConfiguration[i];
            }
        }

        return null;
    }



    public void UpgradeWeaponType(WeaponType type)
    {
        SingleWeaponSaveData weaponCurrentData = null;
        foreach (var item in _weaponSaveDataCopy.WeaponsSavesCollection)
        {
            if (item.WeaponType != type)
            {
                continue;
            }

            weaponCurrentData = item;
        }


        if (weaponCurrentData == null)
        {
            Debug.LogError("[###] ERROR! UPGRADING FAIL: " + type);
            return;
        }



        int currentLevel = weaponCurrentData.LevelNumber;
        int currentStepIndex = weaponCurrentData.StepNumber;
        var generalWeaponData = GetWeaponType_GameData(type);


        var levelData = GetWeapon_LevelsConfig(generalWeaponData, currentLevel);
        

        int currentLevelStepsAmount = levelData.LevelStepsData.Count;
        int currentLevelMaxStepIndex = currentLevelStepsAmount-1;
        int maxLevelNumber = generalWeaponData.WeaponLevelConfiguration.Count - 1;


        int newLevelNumber = 0;
        int newStepIndex = 0;

        



        currentStepIndex++;

        
        // If next step is bigger then maxIndex -> move to next level
        if (currentStepIndex > currentLevelMaxStepIndex)
        {

            // Increase level number
            int nextLevel = currentLevel + 1;


            // If we are out of bounds -> save max top value
            if (nextLevel > maxLevelNumber)
            {
                Debug.LogError("[###] ----------------");
                newLevelNumber = maxLevelNumber;
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


        PlayerDataManager.Instance.UpgradeWepon(newLevelNumber, newStepIndex, 1);
    }
}

public enum WeaponType
{
    None = 0,
    AssaultRifle = 1,
    RocketLauncher = 2,
}
























































public static class WeaponSystemDataConfig {


    public static List<WeaponType_GameData> WeaponData = new List<WeaponType_GameData>()
    {



        // General config about weapon -> Assault-Rifle
        new WeaponType_GameData()
        {
            WeaponType = WeaponType.AssaultRifle,
            WeaponName = "M4A1",
            IsUnlockedFromStart = true,
            UnlockPrice = 0,




            // Defines amount of levels and stats for weapon
            WeaponLevelConfiguration = new List<Weapon_LevelsConfig>()
            {
                // Level 0
                new Weapon_LevelsConfig() 
                {
                    LevelNumber = 0,

                    // Define amount of steps for this level
                    LevelStepsData = new List<Weapon_LevelStepData>()
                    {
                        new Weapon_LevelStepData() {
                            UpgradeCost = 100,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 10, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 150,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 11, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 200,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 12, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 250,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 13, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 300,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 14, Cooldown = 1},
                        }
                    }
                },



                // Level 1
                new Weapon_LevelsConfig() 
                {
                    LevelNumber = 1,

                    // Define amount of steps for this level
                    LevelStepsData = new List<Weapon_LevelStepData>()
                    {
                        new Weapon_LevelStepData() {
                            UpgradeCost = 350,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 10, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 400,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 11, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 450,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 12, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 500,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 13, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 550,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 14, Cooldown = 1},
                        }
                    }
                },



                // Level 2
                new Weapon_LevelsConfig() 
                {
                    LevelNumber = 2,

                    // Define amount of steps for this level
                    LevelStepsData = new List<Weapon_LevelStepData>()
                    {
                        new Weapon_LevelStepData() {
                            UpgradeCost = 600,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 10, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 650,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 11, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 700,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 12, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 750,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 13, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 800,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 14, Cooldown = 1},
                        }
                    }
                },
            }
        },















        // General config about weapon -> Assault-Rifle
        new WeaponType_GameData()
        {
            WeaponType = WeaponType.RocketLauncher,
            WeaponName = "PUSHKA",
            IsUnlockedFromStart = false,
            UnlockPrice = 1,




            // Defines amount of levels and stats for weapon
            WeaponLevelConfiguration = new List<Weapon_LevelsConfig>()
            {
                // Level 0
                new Weapon_LevelsConfig() 
                {
                    LevelNumber = 0,

                    // Define amount of steps for this level
                    LevelStepsData = new List<Weapon_LevelStepData>()
                    {
                        new Weapon_LevelStepData() {
                            UpgradeCost = 100,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 10, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 150,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 11, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 200,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 12, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 250,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 13, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 300,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 14, Cooldown = 1},
                        }
                    }
                },



                // Level 1
                new Weapon_LevelsConfig() 
                {
                    LevelNumber = 1,

                    // Define amount of steps for this level
                    LevelStepsData = new List<Weapon_LevelStepData>()
                    {
                        new Weapon_LevelStepData() {
                            UpgradeCost = 350,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 10, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 400,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 11, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 450,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 12, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 500,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 13, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 550,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 14, Cooldown = 1},
                        }
                    }
                },



                // Level 2
                new Weapon_LevelsConfig() 
                {
                    LevelNumber = 2,

                    // Define amount of steps for this level
                    LevelStepsData = new List<Weapon_LevelStepData>()
                    {
                        new Weapon_LevelStepData() {
                            UpgradeCost = 600,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 10, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 650,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 11, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 700,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 12, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 750,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 13, Cooldown = 1},
                        },
                        
                        new Weapon_LevelStepData() {
                            UpgradeCost = 800,
                            Stats = new Weapon_LevelStepStats() {DamagePoints = 14, Cooldown = 1},
                        }
                    }
                },
            }
        },

    };

}



public class WeaponType_GameData
{
    public WeaponType WeaponType;
    public string WeaponName;
    public int UnlockPrice;
    public bool IsUnlockedFromStart;
    public List<Weapon_LevelsConfig> WeaponLevelConfiguration;




    public Weapon_LevelsConfig GetSpecificLevelConfig(int levelNumber)
    {
        foreach (var data in WeaponLevelConfiguration)
        {
            if (data.LevelNumber == levelNumber)
            {
                return data;
            }
        }


        return null;
    }



}

public class Weapon_LevelsConfig 
{
    // Level number
    public int LevelNumber;

    // Collection of steps
    public List<Weapon_LevelStepData> LevelStepsData;



    public Weapon_LevelStepData GetStepData(int stepNumber)
    {
        if (stepNumber >= LevelStepsData.Count)
        {
            Debug.LogError("[###] ERROR! OUT OF BOUNDS");
            return null;
        }


        for (int i = 0; i < LevelStepsData.Count; i++)
        {
            if (stepNumber == i)
            {
                return LevelStepsData[i];
            }
        }

        return null;
    }
}

public class Weapon_LevelStepData
{
    public int UpgradeCost;
    public Weapon_LevelStepStats Stats;
}

public class Weapon_LevelStepStats
{
    public float DamagePoints;
    public float Cooldown;
}





















// public class AmunitionData
// {
//     public List<EnemyElementDamageBuff> EnemyElementDamageBuffs;
// }
// 
// public class EnemyElementDamageBuff
// {
//     public EnemyElement EnemyElement;
//     public float AdditionalDamage;
// }
// 
// public enum EnemyElement
// {
//     Default = 0,
//     Fire = 1,
//     Water = 2,
// }