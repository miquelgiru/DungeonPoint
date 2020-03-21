using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

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

    private void Awake()
    {
        instance = this;
        Player = new Player(playerStats.HealthPoints, playerStats.DamageAttackPoints);

        //Events
        Pathfinder.Instance.OnPathfindFinished += NewPathFound;
        InputManager.Instance.OnTileSelected += TileSelected;
        InputManager.Instance.OnEnemySelected += EnemySelected;

    }
    // Use this for initialization
    void Start()
    {
        GridManager.Instance.Init();
        Pathfinder.Instance.WantToFindNewPath(GridManager.Instance.EntryPoint, GridManager.Instance.ExitPoint);

        StartCoroutine(LateStart(1.0f));
    }

    IEnumerator LateStart(float waitTime)
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

    public List<Element> GetElementsList()
    {
        return allElements;
    }

    public void RegisterTileElementPair(GridTile tile, Element element)
    {
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
        Debug.Log(nodes.Count);
    }

    public void TileSelected(GridTile tile)
    {
        Player.MovePlayer(tile.Node.X, tile.Node.Z);
        GridManager.Instance.UpdateTileNodes(tile);

        if (tileElements.ContainsKey(tile)){

            Item item = tileElements[tile] as Item;

            if(item != null)
                item.ItemSelected(Player);
        }
    }

    public void EnemySelected(Enemy enemy)
    {
        Player.AttackEnemy(enemy);
    }

    public void DungeonPassed()
    {
        Debug.Log("DungeonPassed survived");
    }
}
