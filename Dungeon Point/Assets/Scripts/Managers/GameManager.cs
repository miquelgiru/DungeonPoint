using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Instance
    private static GameManager instance;
    public static GameManager Instance { get { return instance; } }
    #endregion

    [SerializeField] PlayerStats playerStats;
    public Player Player;

    private List<Element> allElements = new List<Element>();
    private Dictionary<GridTile, Element> tileElements = new Dictionary<GridTile, Element>();

    [SerializeField] GameObject GameOverPopup;

    [SerializeField] Text HealthHUD;
    [SerializeField] Text AttackHUD;
    [SerializeField] Text MovesHUD;

    private int indexLevel = 0;

    private void Awake()
    {
        instance = this;
        Player = new Player(playerStats.HealthPoints, playerStats.DamageAttackPoints);
    }
    // Use this for initialization
    void Start()
    {
        //Events
        Pathfinder.Instance.OnPathfindFinished += NewPathFound;
        InputManager.Instance.OnTileSelected += TileSelected;
        InputManager.Instance.OnEnemySelected += EnemySelected;

        LoadLevel();
    }

    private void LoadLevel()
    {
        GridManager.Instance.InitLevel(indexLevel);
        indexLevel++;
        Pathfinder.Instance.WantToFindNewPath(GridManager.Instance.EntryPoint, GridManager.Instance.ExitPoint);

        StartCoroutine(LateStart(1.0f));
    }

    private IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);

        tileElements[GridManager.Instance.EntryPoint.Tile].gameObject.SetActive(true);
        TileSelected(GridManager.Instance.EntryPoint.Tile);
    }

    public void RegisterElement(Element element)
    {
        if (!allElements.Contains(element))
            allElements.Add(element);
    }

    public void UnregisterElement(Element element)
    {
        if (allElements.Contains(element))
            allElements.Remove(element);
    }

    public List<Element> GetElementsList()
    {
        return allElements;
    }

    public void RegisterTileElementPair(GridTile tile, Element element)
    {
        if(!tileElements.ContainsKey(tile))
            tileElements.Add(tile, element);
    }

    public void UnregisterTileElementPair(GridTile tile)
    {
        if(tileElements.ContainsKey(tile))
            tileElements.Remove(tile);
    }

    public void UnregisterTileElementPair(Element element)
    {
        if (tileElements.ContainsValue(element))
        {
            var item = tileElements.First(pair => pair.Value == element);
            item.Key.Node.HasEnemy = false;
            tileElements.Remove(item.Key);
        }
    }

    public Dictionary<GridTile, Element> GetTilesElementsPairs()
    {
        return tileElements;
    }

    public void NewPathFound(List<GridNode> nodes)
    {
        MovesHUD.text = nodes.Count.ToString();
    }

    public void TileSelected(GridTile tile)
    {
        Pathfinder.Instance.WantToFindNewPath(tile.Node, GridManager.Instance.ExitPoint);

        Player.MovePlayer(tile.Node.X, tile.Node.Z);
        GridManager.Instance.UpdateTileNodes(tile);

        if (tileElements.ContainsKey(tile)){

            Item item = tileElements[tile] as Item;

            if(item != null)
                item.ItemSelected(Player);
        }

        UpdatPlayerHUD();
    }

    public void EnemySelected(Enemy enemy)
    {
        Player.AttackEnemy(enemy);
    }

    public void DungeonPassed()
    {
        Debug.Log("Dungeon survived");
        ClearLevel();

        LoadNextDungeon();
    }

    private void ClearLevel()
    {
        tileElements.Clear();
        GridManager.Instance.ClearGrid();
        Pathfinder.Instance.Path = null;

        for(int i = allElements.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(allElements[i].gameObject);
        }

        allElements.Clear();
    }

    private void LoadNextDungeon()
    {
        if(indexLevel < GridManager.Instance.levelsCount)
        {
            LoadLevel();
        }
        else
        {
            SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
        }
    }

    private void UpdatPlayerHUD()
    {
        HealthHUD.text = Player.PlayerHealth.ToString();
        AttackHUD.text = Player.PlayerAttack.ToString();
    }

    public void PlayerDead()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        GameOverPopup.SetActive(true);
        yield return new WaitForSeconds(3.0f);

        SceneManager.LoadScene("MenuScene", LoadSceneMode.Single);
    }
}
