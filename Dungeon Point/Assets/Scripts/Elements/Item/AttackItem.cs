using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : Item
{
    public override void ItemSelected(Player player)
    {
        player.PlayerAttack += 1;
        Debug.Log(player.PlayerAttack);
        gameObject.SetActive(false);
    }
}
