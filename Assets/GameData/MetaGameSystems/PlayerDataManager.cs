using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class PlayerDataManager : Manager<PlayerDataManager>
{
    public PlayerSaveData PlayerData;

    [HideInInspector] public UnityEvent OnDataChanged = new UnityEvent();




    
    public override void init()
    {
        ReadPlayerData();
    }






    void ReadPlayerData()
    {
        PlayerData = new PlayerSaveData
        {
            CurrencyData = new PlayerSaveData_Currency() { CoinsAmount = 350, CrystalsAmount = 10 },
            ArmourData = new PlayerSaveData_Armour() { ArmourLevel = 0, ArmourLevelStep = 0 },
            HealthData = new PlayerSaveData_Health() { HelathLevel = 0, HelathLevelStep = 0 },
            WeaponData = new PlayerSaveData_Weapon()
            {
                WeaponsSavesCollection = new List<SingleWeaponSaveData>() {
                    new SingleWeaponSaveData()
                    {
                        WeaponType = WeaponType.AssaultRifle,
                        IsUnlocked = true,
                        LevelNumber = 0,
                        StepNumber = 0,
                    }
                }
            },
        };
    }

    // Save player data to backend or file
    public void SavePlayerData()
    {

    }








    public void TryToUpgradeArmour(int price)
    {
        if (PlayerData.CurrencyData.CoinsAmount < price)
        {
            Debug.Log("[Player Data Manager] Can not upgrade armour. Not enough coins.");
            return;
        }

        var upgradedData = ArmourDataManager.Instance.GetUpgradedData();
        if (upgradedData == null)
        {
            Debug.LogError("[Player Data Manager] Error Armour Upgrading. Aborted.");
            return;
        }


        Debug.Log("[Player Data Manager] Upgrade armour operation.");
        PlayerData.CurrencyData.CoinsAmount -= price;
        PlayerData.ArmourData = upgradedData;


        // Save to backend
        SavePlayerData();


        OnDataChanged.Invoke();
    }

    public void TryToUpgradeHealth(int price)
    {
       if (PlayerData.CurrencyData.CoinsAmount < price)
        {
            Debug.Log("[Player Data Manager] Can not upgrade health. Not enough coins.");
            return;
        }

        var upgradedData = HealthDataManager.Instance.GetUpgradedData();
        if (upgradedData == null)
        {
            Debug.LogError("[Player Data Manager] Error Health Upgrading. Aborted.");
            return;
        }


        Debug.Log("[Player Data Manager] Upgrade health operation.");
        PlayerData.CurrencyData.CoinsAmount -= price;
        PlayerData.HealthData = upgradedData;


        // Save to backend
        SavePlayerData();


        OnDataChanged.Invoke();
    }




    // Weapon data manipulations
    public void UnlockWeapon(WeaponType type, int price)
    {
        var freshWeaponData = new SingleWeaponSaveData()
        {
            WeaponType = type,
            IsUnlocked = true,
            LevelNumber = 0,
            StepNumber = 0,
        };


        PlayerData.WeaponData.WeaponsSavesCollection.Add(freshWeaponData);
        PlayerData.CurrencyData.CoinsAmount -= price;


        // Save to backend
        SavePlayerData();
        OnDataChanged.Invoke();
    }

    public void UpgradeWepon(WeaponType type, int newLevel, int newStep, int price)
    {

        PlayerData.CurrencyData.CoinsAmount -= price;
        
        foreach (var item in PlayerData.WeaponData.WeaponsSavesCollection)
        {
            if (item.WeaponType == type)
            {
                item.StepNumber = newStep;
                item.LevelNumber = newLevel;
            }
        }


        // Save to backend
        SavePlayerData();
        OnDataChanged.Invoke();
    }
}