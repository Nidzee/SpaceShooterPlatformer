using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UserInterfaceManager : MonoBehaviour
{
    public static UserInterfaceManager Instance;

    private void Awake()
    {
        Instance = this;
    }



    [SerializeField] ArmourTab _armourTab;
    [SerializeField] HealthTab _healthTab;
    [SerializeField] CurrencyPanel _currencyPanel;
    [SerializeField] UniversalButton _testButton;

    [SerializeField] ArsenalTabContainer _arsenalTab;

    public void InitManager()
    {
        _armourTab.InitTab();
        _healthTab.InitTab();
        _currencyPanel.InitPanel();


        _testButton.OnClick.AddListener(testOpenPopUp);
    }

    
    void testOpenPopUp()
    {
        _arsenalTab.init();


        // PopUpController.OpenSpecificScene<HealthPopUp>("HealthPopUp", (HealthPopUp p) => {
        //     p.showPopUp();
        //     p.init();
        // });
    }
}