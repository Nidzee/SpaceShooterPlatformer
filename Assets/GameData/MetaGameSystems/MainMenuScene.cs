using UnityEngine;
using System.Linq;
using System;
using System.Collections.Generic;


public class TabButtonTypeTabObject 
{
    public TabButton TabButton;
    public MainMenuSceneTab Tab;
}

public class MainMenuScene : MonoBehaviour
{
    public static MainMenuScene Instance;
    private void Awake(){Instance = this;}



    [Header("Tab buttons config")]
    [SerializeField] TabButton _arsenalTabButton;
    [SerializeField] TabButton _healthArmourTabButton;


    [Header("Tabs config")]
    [SerializeField] ArsenalTabContainer _arsenalTab;
    [SerializeField] HealthArmourTab _healthArmourTab;





    [SerializeField] CurrencyPanel _currencyPanel;

    Dictionary<TabButtonType, TabButtonTypeTabObject> _tabButtonTypeTabObject;




    public void InitManager()
    {
        // Specify relative tab-button with tab-object
        _tabButtonTypeTabObject = new Dictionary<TabButtonType, TabButtonTypeTabObject>() {
            {
                TabButtonType.ArsenalTab, new TabButtonTypeTabObject() {
                    Tab = _arsenalTab,
                    TabButton = _arsenalTabButton,
                }
            },

            {
                TabButtonType.HealthArmourTab, new TabButtonTypeTabObject() {
                    Tab = _healthArmourTab,
                    TabButton = _healthArmourTabButton,
                }
            },
        };



        DeactivateAllTabs();



        _arsenalTab.Init();
        _healthArmourTab.Init();


        _arsenalTabButton.Init();
        _arsenalTabButton.OnTabButtonActivated.AddListener(() => OnTabButtonClick(_arsenalTabButton.TabButtonType));

        _healthArmourTabButton.Init();
        _healthArmourTabButton.OnTabButtonActivated.AddListener(() => OnTabButtonClick(_healthArmourTabButton.TabButtonType));






        _currencyPanel.InitPanel();
    }

    void OnTabButtonClick(TabButtonType type)
    {
        DeactivateAllTabs();
        DeactivateAllButtons();


        var data = _tabButtonTypeTabObject[type];
        data.Tab.ThisTabObject.SetActive(true);
        data.Tab.Activate();
        data.TabButton.ActivateButton();
    }

    void DeactivateAllTabs()
    {
        _arsenalTab.gameObject.SetActive(false);
        _healthArmourTab.gameObject.SetActive(false);
    }

    void DeactivateAllButtons()
    {
        _arsenalTabButton.DeactivateButton();
        _healthArmourTabButton.DeactivateButton();
    }
}





























// Logic to open popUp
// PopUpController.OpenSpecificScene<HealthPopUp>("HealthPopUp", (HealthPopUp p) => {
//     p.showPopUp();
//     p.init();
// });