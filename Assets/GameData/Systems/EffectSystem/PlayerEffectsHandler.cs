using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class EffectDisplayData
{
    public EffectType EnvironmnentType;
    public GameObject Icon;
}

public class PlayerEffectsHandler : MonoBehaviour
{
    [SerializeField] List<EffectDisplayData> _availableEffects;
    [SerializeField] PlayerController _player;

    Dictionary<EffectType, List<BasicEffectZone>> _effectsOnPlayer;


    public void Reset()
    {
        // Clear list of current effects
        _effectsOnPlayer = new Dictionary<EffectType, List<BasicEffectZone>>();
        
        // Deactivate all particles
        DeactivateAllParticles();
    }









    public void ApplyEffect(BasicEffectZone effectZone)
    {
        // Check if effect is on player
        if (_effectsOnPlayer.TryGetValue(effectZone.EffectData.EffectType, out var zonesCollection))
        {

            // Check if this zone is on player
            if (zonesCollection.Contains(effectZone))
            {
                Debug.Log("This effect zone is already in dictionary");
                return;
            }
            else
            {
                Debug.Log("Add another same zone");
                zonesCollection.Add(effectZone);
            }
        }

        // If not on player -> add to dictionary and launch damage
        else
        {
            _effectsOnPlayer[effectZone.EffectData.EffectType] = new List<BasicEffectZone>();
            _effectsOnPlayer[effectZone.EffectData.EffectType].Add(effectZone);
            ApplyEffectDamageLogic(effectZone.EffectData);
            UpdateEffectParticle(effectZone.EffectData.EffectType, true);
        }
    }




    public void RemoveEffect(BasicEffectZone effectZone)
    {
        // Check if effect is on player
        if (_effectsOnPlayer.TryGetValue(effectZone.EffectData.EffectType, out var zonesCollection))
        {
            
            // Check if this zone is on player
            if (zonesCollection.Contains(effectZone))
            {
                zonesCollection.Remove(effectZone);

                if (zonesCollection.Count <= 0)
                {
                    UpdateEffectParticle(effectZone.EffectData.EffectType, false);
                    _effectsOnPlayer.Remove(effectZone.EffectData.EffectType);
                }
            }
            else
            {
                Debug.Log("Trying to remove effect from collection: " + effectZone.EffectData.EffectType + " which is not in collection.");
            }
        }

        // If not on player -> add to dictionary and launch damage
        else
        {
            Debug.Log("Trying to remove effect: " + effectZone.EffectData.EffectType + " which is not on player.");
        }
    }






    async void ApplyEffectDamageLogic(EffectData effectData)
    {
        while(IsEffectOnPlayer(effectData.EffectType))
        {
            _player.TakeDamage(effectData.EffectStats.DamagePoints);
            await Task.Delay(effectData.EffectStats.DamageIntervas_Miliseconds);
        }
    }

    bool IsEffectOnPlayer(EffectType type)
    {
        if (_effectsOnPlayer.TryGetValue(type, out var zonesCollection))
        {
            if (zonesCollection.Count <= 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        else
        {
            return false;
        }
    }











    // Effects particles logic
    void UpdateEffectParticle(EffectType type, bool status)
    {
        foreach (var data in _availableEffects)
        {
            if (data.EnvironmnentType == type)
            {
                data.Icon.gameObject.SetActive(status);
                return;
            }
        }

        Debug.LogError("Missing particle object for: " + type);
    }

    void DeactivateAllParticles()
    {
        foreach (var data in _availableEffects)
        {
            data.Icon.gameObject?.SetActive(false);
        }
    }
}