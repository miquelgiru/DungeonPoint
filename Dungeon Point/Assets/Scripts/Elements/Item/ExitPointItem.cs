using UnityEngine;
using System.Collections;

public class ExitPointItem : Item
{
    public override void ItemSelected(Player player)
    {
        GameManager.Instance.DungeonPassed();
    }
}
