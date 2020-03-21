using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public override void ItemSelected(Player player)
    {
        player.PlayerHealth += 1;
        Debug.Log(player.PlayerHealth);
        gameObject.SetActive(false);
    }
}
