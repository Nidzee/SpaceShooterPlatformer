using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.UI;
using UnityEngine;
using System.Linq;


[System.Serializable]
public class EffectDisplayData
{
    public EffectType EnvironmnentType;
    public GameObject ParticleObject;
    public GameObject Icon;
}


public class PlayerEffectsHandler : MonoBehaviour
{
    [SerializeField] List<EffectDisplayData> _availableEffects;
    [SerializeField] PlayerController _player;

    List<EffectData> _effectsOnPlayer;


    public void Reset()
    {
        // Clear list of current effects
        _effectsOnPlayer = new List<EffectData>();
        
        // Deactivate all particles
        DeactivateAllParticles();
    }









    public void ApplyEffect(EffectData effectData)
    {
        // Check if effect is already on player
        var effectDataOnPlayer = _effectsOnPlayer.FirstOrDefault(i => i.EffectType == effectData.EffectType);
        if (effectDataOnPlayer != null)
        {
            Debug.Log("Effect: " + effectData.EffectType + " is already on player.");
            return;
        }


        // Add efect to player effects container
        _effectsOnPlayer.Add(effectData);

        // Try to activate particles
        UpdateEffectParticle(effectData.EffectType, true);

        // Start apply damage logic
        ApplyEffectDamageLogic(effectData);
    }

    public void RemoveEffect(EffectData effectData)
    {
        var effetOnPlayerData = _effectsOnPlayer.FirstOrDefault(i => i.EffectType == effectData.EffectType);
        
        if (effetOnPlayerData != null)
        {
            Debug.Log("Remove effect: " + effetOnPlayerData.EffectType);
            UpdateEffectParticle(effetOnPlayerData.EffectType, false);
            _effectsOnPlayer.Remove(effetOnPlayerData);
        }
        else
        {
            Debug.Log("Trying to remove effect: " + effectData.EffectType + " which is not on player.");
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
        var effectOnPlayerData = _effectsOnPlayer.FirstOrDefault(i => i.EffectType == type);
        
        if (effectOnPlayerData != null)
        {
            return true;
        }

        return false;
    }











    // Effects particles logic
    void UpdateEffectParticle(EffectType type, bool status)
    {
        foreach (var data in _availableEffects)
        {
            if (data.EnvironmnentType == type)
            {
                if (data.ParticleObject != null)
                {
                    data.ParticleObject?.SetActive(status);
                }
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
            if (data.ParticleObject != null)
            {
                data.ParticleObject.SetActive(false);
            }

            data.Icon.gameObject?.SetActive(false);
        }
    }
}