using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class WeaponTypeScroll : MonoBehaviour
{
    [SerializeField] WeaponTypeWidget _widgetPrefab;
    [SerializeField] Transform _parent;

    List<WeaponTypeWidget> _weaponTypeWidgets = new List<WeaponTypeWidget>();
    WeaponTypeWidget _lastSelectedWidget;

    public UnityEvent<WeaponTypeWidget> OnWeaponWidgetSelected = new UnityEvent<WeaponTypeWidget>();





    void RefreshData()
    {
        foreach(var item in _weaponTypeWidgets)
        {
            item.RefreshWidget();
        }
    }





    public void InitWidget()
    {
        WeaponDataManager.Instance.OnDataChanged_Weapon.AddListener(RefreshData);

        _lastSelectedWidget = null;
        _weaponTypeWidgets = new List<WeaponTypeWidget>();

        InstantiateAllWidgets();
        selectDefaultWidget();
    }

    void InstantiateAllWidgets()
    {
        // Get all data about weapons
        var weaponDatas = WeaponDataManager.Instance.GetAllWeaponTypes();


        foreach (var type in weaponDatas)
        {
            WeaponTypeWidget widget = Instantiate(_widgetPrefab, _parent).GetComponent<WeaponTypeWidget>();
            
            widget.init(WeaponDataManager.Instance.GetWeaponType_GameData(type));
            
            widget.OnWidgetClick.AddListener(HandleClickOnWidget);
            widget.OnUnlockClick.AddListener(HandleUnlockClick);

            _weaponTypeWidgets.Add(widget);
        }
    }

    public void selectDefaultWidget()
    {
        var firstWidget = _weaponTypeWidgets.FirstOrDefault();
        if (firstWidget == null)
        {
            Debug.LogError("[###] Error! Widget not found in collection.");
            return;
        }


        HandleClickOnWidget(firstWidget);
    }

    
    void HandleClickOnWidget(WeaponTypeWidget clickedWidget)
    {

        // Skip same widget click
        if (_lastSelectedWidget == clickedWidget)
        {
            return;
        }


        // Deselect last widget
        if (_lastSelectedWidget != null)
        {
            _lastSelectedWidget.UpdateOutline(false);
        }

        // Mark new widget as selected
        _lastSelectedWidget = clickedWidget;
        clickedWidget.UpdateOutline(true);


        // Fire event that this widget was selected
        OnWeaponWidgetSelected.Invoke(clickedWidget);
    }

    void HandleUnlockClick(WeaponTypeWidget widget)
    {
        WeaponDataManager.Instance.UnlockWeponType(widget.WidgetData.WeaponType);
        HandleClickOnWidget(widget);
    }
}
