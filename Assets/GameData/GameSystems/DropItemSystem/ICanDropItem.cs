using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICanDropItem
{
    public LootBag lootBag {get;}
    public void DropLoot();
}