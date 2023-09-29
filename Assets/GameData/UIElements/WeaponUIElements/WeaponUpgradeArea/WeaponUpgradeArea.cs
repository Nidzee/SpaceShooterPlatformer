using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class WeaponUpgradeArea : MonoBehaviour
{
    [Header("Visuals data")]
    [SerializeField] TMP_Text _weaponName;
    [SerializeField] UnityEngine.UI.Image _weponIcon;


    [Header("Wepon Stats")]
    [SerializeField] Slider _upgradeSlider;
    [SerializeField] TMP_Text _levelLabel;
    [SerializeField] TMP_Text _stepLabel;


    [SerializeField] UniversalButton _weponUpgradeButton;



    WeaponType_GameData _currentDataUpgrading;



    public void InitWidget()
    {
        ConnectSignals();
    }

    public void SetWeaponDataForDisplay(WeaponType_GameData data)
    {
        _currentDataUpgrading = data;
        SetWeponData();
    }

    void SetWeponData()
    {
        // Get player save data about this weapon type
        var weaponData = PlayerDataManager.Instance.PlayerData.WeaponData;
        SingleWeaponSaveData savedDataAboutThisWeapon = null;
        foreach (var item in weaponData.WeaponsSavesCollection)
        {
            if (item.WeaponType == _currentDataUpgrading.WeaponType)
            {
                savedDataAboutThisWeapon = item;
            }
        }
        if (savedDataAboutThisWeapon == null)
        {
            Debug.LogError("[###] Error! Weapon type is not in collection: " + _currentDataUpgrading.WeaponType);
            return;
        }









        int savedLevelNumber = savedDataAboutThisWeapon.LevelNumber;
        int savedStepNumber = savedDataAboutThisWeapon.StepNumber;
        var weaponLevelData = _currentDataUpgrading.GetSpecificLevelConfig(savedLevelNumber);




        // Set basic visuals
        _weaponName.text = _currentDataUpgrading.WeaponName;
        _levelLabel.text = savedLevelNumber.ToString();
        _stepLabel.text = savedStepNumber.ToString();




        // Init upgrade slider
        int stepsAmount = weaponLevelData.LevelStepsData.Count - 1;
        _upgradeSlider.maxValue = stepsAmount;
        _upgradeSlider.value = savedStepNumber;




        // Init upgrade button
        var stepData = weaponLevelData.GetStepData(savedStepNumber);
        int upgradeCost = stepData.UpgradeCost;
        _weponUpgradeButton.SetLabel(upgradeCost.ToString());
    }







    
    void ConnectSignals()
    {
        // If upgrade pressed -> try to upgrade armour
        _weponUpgradeButton.OnClick.AddListener(clickOnUpgradeButton);

        // If armour data was changed -> refresh data and purchase button
        WeaponDataManager.Instance.OnDataChanged_Weapon.AddListener(RefreshWidgetAfterUpgrade);
    }

    void RefreshWidgetAfterUpgrade()
    {
        // We upgrade only this weapon type -> so we need to update this visuals
        SetWeponData();
    }

    void clickOnUpgradeButton()
    {
        WeaponDataManager.Instance.UpgradeWeaponType(_currentDataUpgrading.WeaponType);
    }
}
