using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnergyTank
{
    private DateTime _reloadFinishTime;
    private DateTime _reloadStartTime;
}

public class ReloadTimeData
{
    public int Days;
    public int Hours;
    public int Minutes;
    public int Seconds;
}