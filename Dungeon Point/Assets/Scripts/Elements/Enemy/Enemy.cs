using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Element
{
    public int HealthPoints;
    public int AttackDamage;

    [SerializeField] Text HealthText;
    [SerializeField] Text AttackText;

    public List<GridTile> blockedTiles = new List<GridTile>();


    // Start is called before the first frame update
    void Start()
    {
        Node.HasEnemy = true;
        UpdateUIInfo();
        base.Start();
    }

    public void Attacked(int damage, Player player)
    {
        HealthPoints -= damage;
        UpdateUIInfo();
        if (HealthPoints <= 0)
            Die();
        else
            player.Attacked(AttackDamage);
    }

    private void Die()
    {
        UnLockTiles();
        GameManager.Instance.UnregisterTileElementPair(this);
        DestroyImmediate(gameObject);
    }

    private void UpdateUIInfo()
    {
        HealthText.text = HealthPoints.ToString();
        AttackText.text = AttackDamage.ToString();
    }

    private void UnLockTiles()
    {
        foreach(GridTile t in blockedTiles)
        {
            t.SpriteOverlay.gameObject.SetActive(false);
            t.Node.IsBlocked = false;
        }
    }
}
