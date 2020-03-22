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


    [Header("Info Popup")]
    [SerializeField] GameObject Popup;
    [SerializeField] Text Name;
    public string name;
    [SerializeField] Text Description;
    public string description;
    [SerializeField] Text HealthInfo;
    [SerializeField] Text AttackInfo;

    public List<GridTile> blockedTiles = new List<GridTile>();


    // Start is called before the first frame update
    void Start()
    {
        Node.HasEnemy = true;
        UpdateUIInfo();
        SetPopupdata();
        base.Start();
    }

    private void OnEnable()
    {
        StartCoroutine(ShowEnemyInfo());
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
        GameManager.Instance.UnregisterElement(this);
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

    private void SetPopupdata()
    {
        Name.text = name;
        Description.text = description;
        HealthInfo.text = HealthPoints.ToString();
        AttackInfo.text = AttackDamage.ToString();
        Popup.SetActive(false);
    }

    private IEnumerator ShowEnemyInfo()
    {
        Popup.SetActive(true);
        yield return new WaitForSeconds(3.0f);
        Popup.SetActive(false);
    }
}
