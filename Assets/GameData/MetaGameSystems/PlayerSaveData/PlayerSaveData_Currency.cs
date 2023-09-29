using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveData_Currency
{
    public int CrystalsAmount;
    public int CoinsAmount;

    
    public PlayerSaveData_Currency GetCopy()
    {
        PlayerSaveData_Currency result = new PlayerSaveData_Currency();
        result.CrystalsAmount = this.CrystalsAmount;
        result.CoinsAmount = this.CoinsAmount;

        return result;
    }

    public bool IsEqual(PlayerSaveData_Currency compareData)
    {
        if (compareData.CrystalsAmount != this.CrystalsAmount)
        {
            return false;
        }

        if (compareData.CoinsAmount != this.CoinsAmount)
        {
            return false;
        }

        return true;
    }
}