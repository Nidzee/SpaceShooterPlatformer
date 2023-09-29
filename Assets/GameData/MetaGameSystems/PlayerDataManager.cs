using System.Collections.Generic;
using System.Collections;
using UnityEngine.Events;
using UnityEngine;

public class PlayerDataManager : MonoBehaviour
{
    public static PlayerDataManager Instance;
    
    void Awake()
    {
        Instance = this;
    }



    public PlayerSaveData PlayerData;

    [HideInInspector] public UnityEvent OnDataChanged;



    public void InitManager()
    {
        ReadPlayerData();
    }

    // Read player data from backend or file
    void ReadPlayerData()
    {
        PlayerData = new PlayerSaveData();

        PlayerData.ArmourData = new PlayerSaveData_Armour() {ArmourLevel = 0, ArmourLevelStep = 0};
        PlayerData.HealthData = new PlayerSaveData_Health() {HelathLevel = 0, HelathLevelStep = 0};

        PlayerData.WeaponData = new PlayerSaveData_Weapon() {WeaponsSavesCollection = new List<SingleWeaponSaveData>() {
            new SingleWeaponSaveData()
            {
                WeaponType = WeaponType.AssaultRifle,
                IsUnlocked = true,
                LevelNumber = 0,
                StepNumber = 0,
            }
        }};


        PlayerData.CurrencyData = new PlayerSaveData_Currency() {CoinsAmount = 350, CrystalsAmount = 10};
    }


    // Save player data to backend or file
    public void SavePlayerData()
    {

    }






    // Currency operations
    public void TryToAddCurrency(CurrencyType type, int addAmount)
    {
        if (type == CurrencyType.Coins)
        {
            PlayerData.CurrencyData.CoinsAmount += addAmount;
        } 
        else if (type == CurrencyType.Crystals)
        {
            PlayerData.CurrencyData.CrystalsAmount += addAmount;
        }

        // Save to backend
        SavePlayerData();

        OnDataChanged.Invoke();
    }

    public void TryToRemoveCurrency(CurrencyType type, int removeAmount)
    {
        if (type == CurrencyType.Coins)
        {
            if (PlayerData.CurrencyData.CoinsAmount < removeAmount)
            {
                Debug.Log("[Player Data Manager] Can not remove coins. Not enough coins.");
                return;
            }

            PlayerData.CurrencyData.CoinsAmount -= removeAmount;
        } 
        else if (type == CurrencyType.Crystals)
        {
            if (PlayerData.CurrencyData.CrystalsAmount < removeAmount)
            {
                Debug.Log("[Player Data Manager] Can not remove crystals. Not enough coins.");
                return;
            }
            
            PlayerData.CurrencyData.CrystalsAmount -= removeAmount;
        }
        
        // Save to backend

        OnDataChanged.Invoke();
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












    public void ShrinkPlayerData()
    {
        ShrinkTheData_Armour();
        ShrinkTheData_Health();
    }

    public void ShrinkTheData_Armour()
    {
        bool isShrinked = false;
        
        // Try to shrink armour data
        PlayerSaveData_Armour savedData = PlayerData.ArmourData.GetCopy();
        PlayerSaveData_Armour shrinkedData = ArmourDataManager.Instance.ShrinkToConfigBounds(savedData);
        if (!savedData.IsEqual(shrinkedData))
        {
            PlayerData.ArmourData = shrinkedData;
            isShrinked = true;
        }


        // Save to backend shrinked version
        if (isShrinked)
        {
            Debug.Log("[Player-Data-Manager] Data was shrinked on start.");
            OnDataChanged.Invoke();
            SavePlayerData();
        }
    }
    
    public void ShrinkTheData_Health()
    {
        bool isShrinked = false;
        
        // Try to shrink health data
        PlayerSaveData_Health savedData = PlayerData.HealthData.GetCopy();
        PlayerSaveData_Health shrinkedData = HealthDataManager.Instance.ShrinkToConfigBounds(savedData);
        if (!savedData.IsEqual(shrinkedData))
        {
            PlayerData.HealthData = shrinkedData;
            isShrinked = true;
        }


        // Save to backend shrinked version
        if (isShrinked)
        {
            Debug.Log("[Player-Data-Manager] Data was shrinked on start.");
            OnDataChanged.Invoke();
            SavePlayerData();
        }
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

    public void UpgradeWepon(int newLevel, int newStep, int price)
    {

        PlayerData.CurrencyData.CoinsAmount -= price;
        
        foreach (var item in PlayerData.WeaponData.WeaponsSavesCollection)
        {
            item.StepNumber = newStep;
            item.LevelNumber = newLevel;
        }


        // Save to backend
        SavePlayerData();
        OnDataChanged.Invoke();
    }
}