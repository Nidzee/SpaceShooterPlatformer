using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyConfig
{
    public static EnergyConfig Instance {
        get {
            if (_instance == null) {
                _instance = new EnergyConfig();
            }
            
            return _instance;
        }
    }
    static EnergyConfig _instance;


    const int ENERGY_RELOAD_MINUTES = 5;

    public TimeSpan GetEnergyTankReloadSpan()
    {
        return new TimeSpan(0,0,5,0);
    }
}
