using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTab : MonoBehaviour
{
    [SerializeField] HealthDataWidget _healthDataWidget;


    public void InitTab()
    {
        InitWidget();
    }

    void InitWidget()
    {
        _healthDataWidget.InitWidget();
    }
}