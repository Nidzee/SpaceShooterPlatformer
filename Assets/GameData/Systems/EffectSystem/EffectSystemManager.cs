using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectSystemManager : MonoBehaviour
{
    // Class reference
    public static EffectSystemManager Instance;
    
    
    // Data customization in inspector
    [SerializeField] List<EffectConfiguration> _effectConfig = new List<EffectConfiguration>();
    Dictionary<EffectType, EffectStats> _effectTypeStatsCache = new Dictionary<EffectType, EffectStats>();

    
    
    
    void Awake()
    {
        Instance = this;   
        BuildEffectSystemCache(); 
    }

    void BuildEffectSystemCache()
    {
        // Skip if no data provided
        if (_effectConfig == null || _effectConfig?.Count <= 0)
        {
            Debug.LogError("[EffectSystemManager] No data provided.");
            return;
        }



        // Clear the cache
        _effectTypeStatsCache = new Dictionary<EffectType, EffectStats>();

        // Build cache for easy use
        foreach (var config in _effectConfig)
        {
            _effectTypeStatsCache[config.Type] = config.Stats;
        }
    }

    public EffectStats GetEffectStats(EffectType type)
    {
        if (!_effectTypeStatsCache.ContainsKey(type))
        {
            Debug.LogError("[EffectSystemManager] Missing stats for type: " + type);
            return null;
        }
        
        return _effectTypeStatsCache[type];
    }
}






public enum EffectType
{
    Acid,
    ToxicGas,
    Radiation,
}


[System.Serializable]
public class EffectConfiguration
{
    public EffectType Type;
    public EffectStats Stats;
}


[System.Serializable]
public class EffectStats
{
    public DamageType DamageType;
    public float DamagePoints;
    public int DamageIntervas_Miliseconds;
}

public enum DamageType
{
    ReduceHealthOnly,
    ReduceArmourOnly,
    ReduceAll,
}