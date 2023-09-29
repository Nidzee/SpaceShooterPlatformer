using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData
{
    private PlayerSaveData_Health _healthData;
    private PlayerSaveData_Armour _armourData;
    private PlayerSaveData_Weapon _weaponData;
    private PlayerSaveData_Currency _currencyData;


    public PlayerSaveData_Armour ArmourData { get => _armourData; set => _armourData = value;}
    public PlayerSaveData_Health HealthData { get => _healthData; set => _healthData = value;}
    public PlayerSaveData_Weapon WeaponData { get => _weaponData; set => _weaponData = value;}
    public PlayerSaveData_Currency CurrencyData { get => _currencyData; set => _currencyData = value;}
}