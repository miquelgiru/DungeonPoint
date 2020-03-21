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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MovePlayer(int posX, int posZ)
    {
        PlayerPosition = new Vector3(posX, 0, posZ);
    }

    public void AttackEnemy(Enemy enemy)
    {
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

    }
}
