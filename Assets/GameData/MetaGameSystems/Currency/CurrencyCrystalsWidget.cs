using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyCrystalsWidget : MonoBehaviour
{
    [SerializeField] TMP_Text _crystalsLabel;


    public void SetCurrencyDisplay()
    {
        int crystals = PlayerDataManager.Instance.PlayerData.CurrencyData.CrystalsAmount;
        _crystalsLabel.text = crystals.ToString();
    }
}
