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
        PlayerDataManager.Instance.InitManager();

        ArmourDataManager.Instance.InitManager();
        HealthDataManager.Instance.InitManager();
        CurrencyDataManager.Instance.InitManager();





        WeaponDataManager.Instance.init();







        PlayerDataManager.Instance.ShrinkPlayerData();
        
        // Temporary
        UserInterfaceManager.Instance.InitManager();
    }
}