using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UniversalButton;

[CreateAssetMenu(fileName = "UniversalButtonConfigAsset", menuName = "UniversalButtonCofig")]
public class UniversalButtonConfigAsset : ScriptableObject
{
    [SerializeField] List<ButtonStyleConfig> _configsList;

    public ButtonVisualsConfig GetButtonConfig(ButtonStyle style)
    {
        foreach (var config in _configsList)
        {
            if (config.ButtonStyle.Equals(style))
            {
                return config.VisualsConfig;
            }
        }

        return null;
    }
}
