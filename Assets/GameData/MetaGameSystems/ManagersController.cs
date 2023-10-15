using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagersController : MonoBehaviour
{
    public static ManagersController Instance;

    public void Awake()
    {
        Instance = this;
    }





    public void Start()
    {
        InitManagers();
    }

    void InitManagers()
    {
        // Read player data
        // Contains actual player data (armour/currency/health/arsenal)
        PlayerDataManager.Instance.init();




        HealthDataManager.Instance.init();
        ArmourDataManager.Instance.init();
        CurrencyDataManager.Instance.init();
        WeaponDataManager.Instance.init();





        // Temporary
        MainMenuScene.Instance.InitManager();
    }
}