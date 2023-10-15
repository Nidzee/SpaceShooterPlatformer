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
    }

    void RefreshWidget()
    {
        RefreshActualHealthData();
        RefreshPurchaseButton();
    }

    public void ConnectSignals()
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
        PlayerSaveData_Health healthData = HealthDataManager.Instance.GetActualData();
        
        // Set visuals
        _levelLabel.text = healthData.HelathLevel.ToString();
        _stepLabel.text = healthData.HelathLevelStep.ToString();
        _valueLabel.text = HealthDataManager.Instance.GetHealthStepStats(healthData).HealthValue.ToString();
    }

    void RefreshPurchaseButton()
    {
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