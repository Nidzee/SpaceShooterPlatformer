using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystemManager : MonoBehaviour
{
    // Class reference
    public static WeaponSystemManager Instance;

    // Data customization in inspector
    [SerializeField] List<WeaponTypeConfiguration> _weaponConfig;
    Dictionary<WeaponType, WeaponGameData> _weaponTypeDataCache = new Dictionary<WeaponType, WeaponGameData>();



    void Awake()
    {
        Instance = this;

        BuildWeaponTypeDataCache();
    }

    void BuildWeaponTypeDataCache()
    {
        // Skip if no data provided
        if (_weaponConfig == null || _weaponConfig?.Count <= 0)
        {
            Debug.LogError("[WeaponSystemManager] No data provided.");
            return;
        }





        // Clear the cache
        _weaponTypeDataCache = new Dictionary<WeaponType, WeaponGameData>();

        // Build the cahce
        foreach (var config in _weaponConfig)
        {
            if (!config.IsConfigValid())
            {
                Debug.LogError("Invalid weapon config.");
                continue;
            }

            config.BuildCache();
            _weaponTypeDataCache[config.weaponType] = config.weaponGameData;
        }
    }








    public WeaponStats GetWeaponStats(WeaponType type, int level, int step)
    {
        if (!_weaponTypeDataCache.ContainsKey(type))
        {
            Debug.LogException(new System.Exception("No data-type in cache."));
            return null;
        }

        WeaponGameData weaponData = _weaponTypeDataCache[type];
        WeaponStats stats = weaponData.GetWeaponStats(level, step);

        if (stats == null)
        {
            Debug.LogError("No stats by level and step in cache.");
            return null;
        }

        return stats;
    }
}



public enum WeaponType
{
    AssaultRifle = 1,
    RocketLauncher = 2,
}