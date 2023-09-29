using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrencyPanel : MonoBehaviour
{
    [Header("Test currency config")]
    [SerializeField] int _removeAmount;
    [SerializeField] int _addAmount;

    [Header("Coins config")]
    [SerializeField] CurrencyCoinsWidget _coinsWidget;
    [SerializeField] Button _addCoinsButton;
    [SerializeField] Button _removeCoinsButton;

    [Header("Crystals config")]
    [SerializeField] CurrencyCrystalsWidget _crystalsWidget;
    [SerializeField] Button _addCrystalsButton;
    [SerializeField] Button _removeCrystalsButton;


    public void InitPanel()
    {
        // Test purposes
        _addCoinsButton.GetComponentInChildren<Text>().text = "Add: " + _addAmount;
        _removeCoinsButton.GetComponentInChildren<Text>().text = "Remove: " + _removeAmount;
        _addCoinsButton.onClick.AddListener(AddCoins_Test);
        _removeCoinsButton.onClick.AddListener(RemoveCoins_Test);
        _addCrystalsButton.GetComponentInChildren<Text>().text = "Add: " + _addAmount;
        _removeCrystalsButton.GetComponentInChildren<Text>().text = "Remove: " + _removeAmount;
        _addCrystalsButton.onClick.AddListener(AddCrystals_Test);
        _removeCrystalsButton.onClick.AddListener(RemoveCrystals_Test);


        RefreshCurrencyPanel();

        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshCurrencyPanel);
    }




    void AddCoins_Test()
    {
        CurrencyDataManager.Instance.TryToAddCurrency(CurrencyType.Coins, _addAmount);
    }

    void RemoveCoins_Test()
    {
        CurrencyDataManager.Instance.TryToRemoveCurrency(CurrencyType.Coins, _removeAmount);
    }

    void AddCrystals_Test()
    {
        CurrencyDataManager.Instance.TryToAddCurrency(CurrencyType.Crystals, _addAmount);
    }

    void RemoveCrystals_Test()
    {
        CurrencyDataManager.Instance.TryToRemoveCurrency(CurrencyType.Crystals, _removeAmount);
    }




    void RefreshCurrencyPanel()
    {
        _coinsWidget.SetCurrencyDisplay();
        _crystalsWidget.SetCurrencyDisplay();
    }
}