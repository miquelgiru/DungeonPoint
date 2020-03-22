using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItem : Item
{
    public override void ItemSelected(Player player)
    {
        AudioManager.Instance.PlayAudioClipNow(AudioManager.AudioClipType.GRAB_ITEM);
        player.PlayerAttack += 1;
        gameObject.SetActive(false);
    }
}
