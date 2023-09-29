using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmourTab : MonoBehaviour
{
    [SerializeField] ArmourDataWidget _armourDataWidget;


    public void InitTab()
    {
        InitWidget();
    }

    void InitWidget()
    {
        _armourDataWidget.InitWidget();
    }
}