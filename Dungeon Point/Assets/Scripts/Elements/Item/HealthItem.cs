using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public override void ItemSelected(Player player)
    {
        AudioManager.Instance.PlayAudioClipNow(AudioManager.AudioClipType.GRAB_ITEM);
        player.PlayerHealth += 1;
        gameObject.SetActive(false);
    }
}
