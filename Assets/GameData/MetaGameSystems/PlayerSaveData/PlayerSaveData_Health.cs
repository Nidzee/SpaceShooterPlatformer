using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData_Health
{
    public int HelathLevel;
    public int HelathLevelStep;

    public PlayerSaveData_Health GetCopy()
    {
        PlayerSaveData_Health result = new PlayerSaveData_Health();
        result.HelathLevel = this.HelathLevel;
        result.HelathLevelStep = this.HelathLevelStep;

        return result;
    }

    public bool IsEqual(PlayerSaveData_Health compareData)
    {
        if (compareData.HelathLevel != this.HelathLevel)
        {
            return false;
        }

        if (compareData.HelathLevelStep != this.HelathLevelStep)
        {
            return false;
        }

        return true;
    }
}