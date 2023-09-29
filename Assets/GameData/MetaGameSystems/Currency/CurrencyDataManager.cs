using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;


public enum CurrencyType
{
    None = 0,
    Coins = 1,
    Crystals = 2,
}


public class CurrencyDataManager : MonoBehaviour
{
    public static CurrencyDataManager Instance;
    
    void Awake()
    {
        Instance = this;
    }



    PlayerSaveData_Currency _currencyDataCopy;


    // Manager event
    [HideInInspector] public UnityEvent OnDataChanged_Currency;



    public void InitManager()
    {
        _currencyDataCopy = PlayerDataManager.Instance.PlayerData.CurrencyData.GetCopy();
        PlayerDataManager.Instance.OnDataChanged.AddListener(OnPlayerGameDataChanged);
    }

    void OnPlayerGameDataChanged()
    {
        Debug.Log("[Currency-Data-Manager] OnDataChanged triggered.");
        var actualData = PlayerDataManager.Instance.PlayerData.CurrencyData;
        bool isDataEqual = actualData.IsEqual(_currencyDataCopy);

        if (!isDataEqual)
        {
            _currencyDataCopy = actualData.GetCopy();
            Debug.Log("[Currency-Data-Manager] Data updated. OnDataChanged_Currency() triggered.");
            OnDataChanged_Currency.Invoke();
        }
    }





    public void TryToAddCurrency(CurrencyType type, int addAmount)
    {
        PlayerDataManager.Instance.TryToAddCurrency(type, addAmount);
    }
    
    public void TryToRemoveCurrency(CurrencyType type, int removeAmount)
    {
        PlayerDataManager.Instance.TryToRemoveCurrency(type, removeAmount);
    }




    public bool IsEnoughCoins(int coins)
    {
        if (_currencyDataCopy.CoinsAmount >= coins)
        {
            return true;
        }

        return false;
    }
}