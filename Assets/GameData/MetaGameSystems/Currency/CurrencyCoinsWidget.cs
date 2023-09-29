using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyCoinsWidget : MonoBehaviour
{
    [SerializeField] TMP_Text _coinsLabel;


    public void SetCurrencyDisplay()
    {
        int coinsAmount = PlayerDataManager.Instance.PlayerData.CurrencyData.CoinsAmount;
        _coinsLabel.text = coinsAmount.ToString();
    }
}