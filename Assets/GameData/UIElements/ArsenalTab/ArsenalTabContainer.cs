using System.Collections.Generic;
using System.Collections;
using UnityEngine;


public interface MainMenuSceneTab
{
    public GameObject ThisTabObject {get;}
    public void Init();
    public void Activate();
}


public class ArsenalTabContainer : MonoBehaviour, MainMenuSceneTab
{
    [SerializeField] WeaponTypeScroll _weaponTypesScroll;
    [SerializeField] WeaponUpgradeArea _upgradArea;
    GameObject MainMenuSceneTab.ThisTabObject => gameObject;


    public void Init()
    {
        // Connect signals
        _weaponTypesScroll.OnWeaponWidgetSelected.AddListener(HandleClickOnWidget);


        _upgradArea.ConnectSignals();
        _weaponTypesScroll.ConnectSignals();
    }

    public void Activate()
    {
        _weaponTypesScroll.Activate();
    }


    void HandleClickOnWidget(WeaponTypeWidget widgetClicked)
    {
        _upgradArea.SetWeaponDataForDisplay(widgetClicked.WidgetData);
    }
}