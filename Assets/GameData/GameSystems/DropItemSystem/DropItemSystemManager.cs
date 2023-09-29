using System.Collections.Generic;
using UnityEngine;

public class DropItemSystemManager : MonoBehaviour
{
    public static DropItemSystemManager Instance;

    [SerializeField] List<DropItemValueConfig> _dropItemConfig;
    Dictionary<DropItemType, float> _dropItemTypeValueCache = new Dictionary<DropItemType, float>();



    public void Awake()
    {
        Instance = this;
        BuildDropItemSystemCache();
    }

    void BuildDropItemSystemCache()
    {
        // Skip if no data provided
        if (_dropItemConfig == null || _dropItemConfig?.Count <= 0)
        {
            Debug.LogError("[DropItemSystemManager] No data provided.");
            return;
        }


        _dropItemTypeValueCache = new Dictionary<DropItemType, float>();

        foreach (var config in _dropItemConfig)
        {
            _dropItemTypeValueCache[config.itemType] = config.value;
        }
    }

    public float GetDropItemValue(DropItemType type)
    {
        if (!_dropItemTypeValueCache.ContainsKey(type))
        {
            Debug.LogError("[DropItemSystemManager] Missing stats for type: " + type);
            return 0;
        }
        
        return _dropItemTypeValueCache[type];
    }
}

[System.Serializable]
public class DropItemValueConfig
{
    public DropItemType itemType;
    public float value;
}

public enum DropItemType{
    HealthPack,
    ArmourPack,
}