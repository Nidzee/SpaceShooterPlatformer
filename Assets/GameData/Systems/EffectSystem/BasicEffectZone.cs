using UnityEngine;

public class BasicEffectZone : MonoBehaviour
{
    [SerializeField] EffectType _effectType;
    EffectData _effectData;

    public EffectData EffectData => _effectData;



    public void Start()
    {
        InitStats();
    }

    void InitStats()
    {
        // Get stats from manager
        var effectStats = EffectSystemManager.Instance.GetEffectStats(_effectType);
        if (effectStats == null)
        {
            Debug.LogError("Missing stats for effect type: " + _effectType);
            return;
        }

        
        // Set zone effect data
        _effectData = new EffectData()
        {
            EffectType = _effectType,
            EffectStats = effectStats
        };
    }
}