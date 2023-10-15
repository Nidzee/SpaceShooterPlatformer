using UnityEngine.Events;
using UnityEngine;

public class CurrencyDataManager : Manager<CurrencyDataManager>
{
    PlayerSaveData_Currency _currencyDataCopy;

    // Manager event
    [HideInInspector] public UnityEvent OnDataChanged_Currency = new UnityEvent();





    public override void init()
    {
        _currencyDataCopy = PlayerDataManager.Instance.PlayerData.CurrencyData.GetCopy();
        PlayerDataManager.Instance.OnDataChanged.AddListener(OnPlayerGameDataChanged);
    }


    void OnPlayerGameDataChanged()
    {
        var actualData = PlayerDataManager.Instance.PlayerData.CurrencyData;
        bool isDataEqual = actualData.IsEqual(_currencyDataCopy);

        if (!isDataEqual)
        {
            _currencyDataCopy = actualData.GetCopy();
            OnDataChanged_Currency.Invoke();
        }
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

public enum CurrencyType
{
    None = 0,
    Coins = 1,
    Crystals = 2,
}