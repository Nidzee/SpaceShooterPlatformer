using UnityEngine;
using TMPro;

public class ArmourDataWidget : MonoBehaviour
{
    [SerializeField] TMP_Text _levelLabel;
    [SerializeField] TMP_Text _stepLabel;
    [SerializeField] TMP_Text _valueLabel;

    [SerializeField] UniversalButton _upgradeButton;







    public void ConnectSignals()
    {
        // If upgrade pressed -> try to upgrade armour
        _upgradeButton.OnClick.AddListener(ArmourDataManager.Instance.TryToUpgradeArmour);

        // If currency changed -> refresh only currency button
        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshPurchaseButton);

        // If armour data was changed -> refresh data and purchase button
        ArmourDataManager.Instance.OnDataChanged_Armour.AddListener(RefreshWidget);
    }




    public void InitWidget()
    {
        // Set data
        RefreshWidget();
    }

    void RefreshWidget()
    {
        RefreshActualArmourData();
        RefreshPurchaseButton();
    }





    void RefreshActualArmourData()
    {
        // Get actual data
        PlayerSaveData_Armour armourData = ArmourDataManager.Instance.GetActualData();
        
        // Set visuals
        _levelLabel.text = armourData.ArmourLevel.ToString();
        _stepLabel.text = armourData.ArmourLevelStep.ToString();
        _valueLabel.text = ArmourDataManager.Instance.GetArmourStepStats(armourData).ArmourValue.ToString();
    }

    void RefreshPurchaseButton()
    {
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