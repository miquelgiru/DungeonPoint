using UnityEngine;
using System.Collections;

public class ExitPointItem : Item
{
    public override void ItemSelected(Player player)
    {
        AudioManager.Instance.PlayAudioClipNow(AudioManager.AudioClipType.EXIT_DUNGEON);
        GameManager.Instance.DungeonPassed();
    }
}
