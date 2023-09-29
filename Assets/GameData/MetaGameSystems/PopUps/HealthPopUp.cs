using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPopUp : PopUp
{
    [SerializeField] HealthTab _healthTab;

    public void init()
    {
        _healthTab.InitTab();
    }
}