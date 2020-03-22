using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    [Header("Player Stats")]
    public int PlayerHealth;
    public int PlayerAttack;
    public Vector3 PlayerPosition;

    public Player(int health, int attack)
    {
        PlayerHealth = health;
        PlayerAttack = attack;
        PlayerPosition = Vector3.zero;
    }

    public void MovePlayer(int posX, int posZ)
    {
        PlayerPosition = new Vector3(posX, 0, posZ);
    }

    public void AttackEnemy(Enemy enemy)
    {
        AudioManager.Instance.PlayAudioClipNow(AudioManager.AudioClipType.ATTACK);
        enemy.Attacked(PlayerAttack, this);
    }

    public void Attacked(int damage)
    {
        PlayerHealth -= damage;

        if(PlayerHealth <= 0)
        {
            Die();
        }
    }

    public void GetItem(Item item)
    {
        item.ItemSelected(this);
    }

    public void Die()
    {
        GameManager.Instance.PlayerDead();
    }
}
