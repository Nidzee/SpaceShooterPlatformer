using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ArmourDataWidget : MonoBehaviour
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
        RefreshActualArmourData();
        RefreshPurchaseButton();
    }

    void ConnectSignals()
    {
        // If upgrade pressed -> try to upgrade armour
        _upgradeButton.OnClick.AddListener(ArmourDataManager.Instance.TryToUpgradeArmour);

        // If currency changed -> refresh only currency button
        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshPurchaseButton);

        // If armour data was changed -> refresh data and purchase button
        ArmourDataManager.Instance.OnDataChanged_Armour.AddListener(RefreshWidget);
    }




    void RefreshActualArmourData()
    {
        // Get actual data
        PlayerSaveData_Armour armourData = PlayerDataManager.Instance.PlayerData.ArmourData;
        
        // Set visuals
        _levelLabel.text = armourData.ArmourLevel.ToString();
        _stepLabel.text = armourData.ArmourLevelStep.ToString();
        _valueLabel.text = ArmourDataManager.Instance.GetArmourStepStats(armourData).ArmourValue.ToString();
    }

    void RefreshPurchaseButton()
    {
        // Check if top config reached -> disable button to prevent trying to upgrade more than config
        PlayerSaveData_Armour armourData = PlayerDataManager.Instance.PlayerData.ArmourData;
        if (ArmourDataManager.Instance.IsTopConfig(armourData))
        {
            _upgradeButton.SetLabel("Top reached!");
            _upgradeButton.BaseButton.enabled = false;
            return;
        }



        // Set buton upgrade price
        int upgradePrice = ArmourDataManager.Instance.GetUpgradePrice();
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