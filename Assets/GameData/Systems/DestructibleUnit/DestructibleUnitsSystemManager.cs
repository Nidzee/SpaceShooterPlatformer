using System.Collections.Generic;
using UnityEngine;

public class DestructibleUnitsSystemManager : MonoBehaviour
{
    // Class reference
    public static DestructibleUnitsSystemManager Instance;

    // Data customization in inspector
    [SerializeField] List<DestructibleUnitTypeStatsConfiguration> _destructibleUnitConfig = new List<DestructibleUnitTypeStatsConfiguration>();
    Dictionary<DestructibleUnitType, DestructibleUnitStats> _destructibleUnitTypeStatsCache = new Dictionary<DestructibleUnitType, DestructibleUnitStats>();


    public void Awake()
    {
        Instance = this;
        BuildDestructibleUnitsSystemCache();
    }

    
    void BuildDestructibleUnitsSystemCache()
    {
        // Skip if no data provided
        if (_destructibleUnitConfig == null || _destructibleUnitConfig?.Count <= 0)
        {
            Debug.LogError("[DestructibleUnitsSystemManager] No data provided.");
            return;
        }



        // Clear the cache
        _destructibleUnitTypeStatsCache = new Dictionary<DestructibleUnitType, DestructibleUnitStats>();

        // Build cache for easy use
        foreach (var config in _destructibleUnitConfig)
        {
            _destructibleUnitTypeStatsCache[config.unitType] = config.stats;
        }
    }

    public DestructibleUnitStats GetDestructibleUnitStats(DestructibleUnitType type)
    {
        if (!_destructibleUnitTypeStatsCache.ContainsKey(type))
        {
            Debug.LogError("[DestructibleUnitsSystemManager] Missing stats for type: " + type);
            return null;
        }
        
        return _destructibleUnitTypeStatsCache[type];
    }
}

public enum DestructibleUnitType
{
    Crate,
    LootBox,
}

[System.Serializable]
public class DestructibleUnitTypeStatsConfiguration
{
    public DestructibleUnitType unitType;
    public DestructibleUnitStats stats;
}

[System.Serializable]
public class DestructibleUnitStats
{
    public int maxHealthPoints;
}