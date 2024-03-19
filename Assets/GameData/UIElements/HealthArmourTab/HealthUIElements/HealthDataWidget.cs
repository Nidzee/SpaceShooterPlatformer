using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class HealthDataWidget : MonoBehaviour
{
    [SerializeField] TMP_Text _levelLabel;
    [SerializeField] TMP_Text _stepLabel;
    [SerializeField] TMP_Text _valueLabel;
    [SerializeField] Slider _upgradeSlider;

    [SerializeField] UniversalButton _upgradeButton;





    public void ConnectSignals()
    {
        CurrencyDataManager.Instance.OnDataChanged_Currency.AddListener(RefreshWidget);
        HealthDataManager.Instance.OnDataChanged_Health.AddListener(RefreshWidget);


        _upgradeButton.OnClick.AddListener(HealthDataManager.Instance.TryToUpgradeHealth);

    }


    public void InitWidget()
    {
        RefreshWidget();
    }

    void RefreshWidget()
    {
        RefreshActualHealthData();
        RefreshPurchaseButton();
    }




    void RefreshActualHealthData()
    {
        // Get actual data
        PlayerSaveData_Health healthData = HealthDataManager.Instance.GetActualData();
        
        // Set visuals
        _levelLabel.text = healthData.HelathLevel.ToString();
        _stepLabel.text = healthData.HelathLevelStep.ToString();
        _valueLabel.text = HealthDataManager.Instance.GetHealthStepStats(healthData).HealthValue.ToString();


        _upgradeSlider.maxValue = HealthSystemDataConfig.HealthLevelsConfigCollection[healthData.HelathLevel].StepStatsCollection.Count - 1;
        _upgradeSlider.value = healthData.HelathLevelStep;
    }

    void RefreshPurchaseButton()
    {

        // Check if top config was reached
        PlayerSaveData_Health healthData = HealthDataManager.Instance.GetActualData();
        if (HealthDataManager.Instance.IsTopConfig(healthData))
        {
            _upgradeButton.BaseButton.enabled = false;
            _upgradeButton.SetStyle(UniversalButton.ButtonStyle.Gray);
            _upgradeButton.SetLabel("TOP REACHED!");
            return;
        }



        // Check if button can be pressed
        int upgradePrice = HealthDataManager.Instance.GetUpgradePrice();
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