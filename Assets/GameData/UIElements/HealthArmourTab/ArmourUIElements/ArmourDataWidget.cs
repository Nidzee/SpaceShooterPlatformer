using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ArmourDataWidget : MonoBehaviour
{
    [SerializeField] TMP_Text _levelLabel;
    [SerializeField] TMP_Text _stepLabel;
    [SerializeField] TMP_Text _valueLabel;
    [SerializeField] Slider _upgradeSlider;

    [SerializeField] UniversalButton _upgradeButton;







    public void ConnectSignals()
    {
        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshWidget);
        ArmourDataManager.Instance.OnDataChanged_Armour.AddListener(RefreshWidget);



        // If upgrade pressed -> try to upgrade armour
        _upgradeButton.OnClick.AddListener(ArmourDataManager.Instance.TryToUpgradeArmour);
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
    

        _upgradeSlider.maxValue = ArmourSystemDataConfig.ArmourLevelsConfigCollection[armourData.ArmourLevel].StepStatsCollection.Count - 1;
        _upgradeSlider.value = armourData.ArmourLevelStep;
    }
    
    void RefreshPurchaseButton()
    {

        // Check if top config was reached
        PlayerSaveData_Armour armourData = ArmourDataManager.Instance.GetActualData();
        if (ArmourDataManager.Instance.IsTopConfig(armourData))
        {
            _upgradeButton.BaseButton.enabled = false;
            _upgradeButton.SetStyle(UniversalButton.ButtonStyle.Gray);
            _upgradeButton.SetLabel("TOP REACHED!");
            return;
        }




        // Check if button can be pressed
        int upgradePrice = ArmourDataManager.Instance.GetUpgradePrice();
        int coinsAmount = PlayerDataManager.Instance.PlayerData.CurrencyData.CoinsAmount;
        if (coinsAmount >= upgradePrice)
        {
            _upgradeButton.BaseButton.enabled = true;
            _upgradeButton.SetStyle(UniversalButton.ButtonStyle.Green);
        }
        else
        {
            _upgradeButton.BaseButton.enabled = false;
            _upgradeButton.SetStyle(UniversalButton.ButtonStyle.Gray);
        }


        _upgradeButton.SetButtonPrice(upgradePrice);
    }
}