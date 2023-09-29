using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData_Armour
{
    public int ArmourLevel;
    public int ArmourLevelStep;


    public PlayerSaveData_Armour GetCopy()
    {
        PlayerSaveData_Armour result = new PlayerSaveData_Armour();
        result.ArmourLevel = this.ArmourLevel;
        result.ArmourLevelStep = this.ArmourLevelStep;

        return result;
    }

    public bool IsEqual(PlayerSaveData_Armour compareData)
    {
        if (compareData.ArmourLevel != this.ArmourLevel)
        {
            return false;
        }

        if (compareData.ArmourLevelStep != this.ArmourLevelStep)
        {
            return false;
        }

        return true;
    }
}