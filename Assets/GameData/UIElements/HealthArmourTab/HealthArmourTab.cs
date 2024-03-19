using UnityEngine;

public class HealthArmourTab : MonoBehaviour, MainMenuSceneTab
{
    [SerializeField] HealthDataWidget _healthWidget;   
    [SerializeField] ArmourDataWidget _armourWidget;
    GameObject MainMenuSceneTab.ThisTabObject => gameObject;



    public void Init()
    {
        _healthWidget.ConnectSignals();
        _armourWidget.ConnectSignals();
    }

    public void Activate()
    {
        _healthWidget.InitWidget();
        _armourWidget.InitWidget();
    }
}