using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData_Weapon
{
    
    public List<SingleWeaponSaveData> WeaponsSavesCollection;





    
    public PlayerSaveData_Weapon GetCopy()
    {
        PlayerSaveData_Weapon result = new PlayerSaveData_Weapon
        {
            WeaponsSavesCollection = new List<SingleWeaponSaveData>()
        };

        foreach (var item in WeaponsSavesCollection)
        {
            result.WeaponsSavesCollection.Add(new SingleWeaponSaveData(item));
        }

        return result;
    }

    public bool IsEqual(PlayerSaveData_Weapon data)
    {
        return false;
    }
}

public class SingleWeaponSaveData
{
    public WeaponType WeaponType;
    public bool IsUnlocked;
    public int LevelNumber;
    public int StepNumber;

    public SingleWeaponSaveData(){}

    public SingleWeaponSaveData(SingleWeaponSaveData item)
    {
        WeaponType = item.WeaponType;
        StepNumber = item.StepNumber;
        LevelNumber = item.LevelNumber;
        IsUnlocked = item.IsUnlocked;
    }
}