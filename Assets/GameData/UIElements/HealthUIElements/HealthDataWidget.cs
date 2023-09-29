using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HealthDataWidget : MonoBehaviour
{
    [SerializeField] TMP_Text _levelLabel;
    [SerializeField] TMP_Text _stepLabel;
    [SerializeField] TMP_Text _valueLabel;

    [SerializeField] UniversalButton _upgradeButton;


    public void InitWidget()
    {
        // Set data
        RefreshWidget();

        // Connect signals
        ConnectSignals();
    }

    void RefreshWidget()
    {
        RefreshActualHealthData();
        RefreshPurchaseButton();
    }

    void ConnectSignals()
    {
        // If upgrade pressed -> try to upgrade health
        _upgradeButton.OnClick.AddListener(HealthDataManager.Instance.TryToUpgradeHealth);

        // If currency changed -> refresh only currency button
        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshPurchaseButton);

        // If health data was changed -> refresh data and purchase button
        HealthDataManager.Instance.OnDataChanged_Health.AddListener(RefreshWidget);
    }




    void RefreshActualHealthData()
    {
        // Get actual data
        PlayerSaveData_Health healthData = PlayerDataManager.Instance.PlayerData.HealthData;
        
        // Set visuals
        _levelLabel.text = healthData.HelathLevel.ToString();
        _stepLabel.text = healthData.HelathLevelStep.ToString();
        _valueLabel.text = HealthDataManager.Instance.GetHealthStepStats(healthData).HealthValue.ToString();
    }

    void RefreshPurchaseButton()
    {
        // Check if top config reached -> disable button to prevent trying to upgrade more than config
        PlayerSaveData_Health healthData = PlayerDataManager.Instance.PlayerData.HealthData;
        if (HealthDataManager.Instance.IsTopConfig(healthData))
        {
            _upgradeButton.SetLabel("Top reached!");
            _upgradeButton.BaseButton.enabled = false;
            return;
        }



        // Set buton upgrade price
        int upgradePrice = HealthDataManager.Instance.GetUpgradePrice();
        _upgradeButton.SetButtonPrice(upgradePrice);


        // Check if button can be pressed
        int coinsAmount = PlayerDataManager.Instance.PlayerData.CurrencyData.CoinsAmount;
        if (coinsAmount >= upgradePrice)
        {
            _upgradeButton.BaseButton.enabled = true;
        }
        else
        {
            _upgradeButton.BaseButton.enabled = false;
        }
    }
}