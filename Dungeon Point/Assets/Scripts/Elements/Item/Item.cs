using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : Element
{
     protected void Start()
     {
        base.Start();
     }

    public abstract void ItemSelected(Player player);     
}
