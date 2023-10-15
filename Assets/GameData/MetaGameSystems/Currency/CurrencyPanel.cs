using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPanel : MonoBehaviour
{
    [SerializeField] CurrencyCoinsWidget _coinsWidget;
    [SerializeField] CurrencyCrystalsWidget _crystalsWidget;


    public void InitPanel()
    {
        RefreshCurrencyPanel();
        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshCurrencyPanel);
    }

    void RefreshCurrencyPanel()
    {
        _coinsWidget.SetCurrencyDisplay();
        _crystalsWidget.SetCurrencyDisplay();
    }
}