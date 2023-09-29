using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ArsenalTabContainer : MonoBehaviour
{
    [SerializeField] WeaponTypeScroll _weaponTypesScroll;
    [SerializeField] WeaponUpgradeArea _upgradArea;


    public void init()
    {
        _weaponTypesScroll.OnWeaponWidgetSelected.AddListener(HandleClickOnWidget);

        _upgradArea.InitWidget();
        _weaponTypesScroll.InitWidget();
    }

    void HandleClickOnWidget(WeaponTypeWidget widgetClicked)
    {
        _upgradArea.SetWeaponDataForDisplay(widgetClicked.WidgetData);
    }
}