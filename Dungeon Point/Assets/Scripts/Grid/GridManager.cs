﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridManager : MonoBehaviour
{

    #region Instance
    private static GridManager instance;
    public static GridManager Instance { get { return instance; } }
    #endregion

    private GameManager gameManager;

    private GridNode[,] grid;
    private List<GridTile> exploredTiles = new List<GridTile>();
    private List<GridTile> explorableTiles = new List<GridTile>();
    private List<GridTile> blockedTiles = new List<GridTile>();

    private GridTile playerTile;

    [Header("3D map generation")]
    [SerializeField] private List<GridMapSettings> mapConfigurations;
    [SerializeField] private GridElementsConfig elementsConfig;
    [SerializeField] private Transform tilesParent = null;
    [SerializeField] private Transform tilePrefab = null;

    [Header("Tile Materials")]
    [SerializeField] Material PlayerPosMat;
    [SerializeField] Material ExploredMat;
    [SerializeField] Material ExplorableMat;
    [SerializeField] Material UnexplorabledMat;

    [Header("Debug Guizmos")]
    public Vector3 DebugCubeSize = new Vector3(.8f, .8f, .8f);

    private GridMapSettings currentConfig;
    public GridNode EntryPoint;
    public GridNode ExitPoint;


    private void Awake()
    {
        instance = this;
    }

    public void Init()
    {
        gameManager = GameManager.Instance;
        currentConfig = mapConfigurations[0];
        CreateGrid(currentConfig.SizeX, currentConfig.SizeZ);
        CreateMapElements();
    }

    private void CreateGrid(int sizeX, int sizeZ)
    {
        grid = new GridNode[sizeX, sizeZ];

        for (int i = 0; i < sizeX; ++i)
        {
            for (int j = 0; j < sizeZ; ++j)
            {
                //Create grid model
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, 0, j), Quaternion.identity, tilesParent).gameObject;
                GridTile gridTile = tile.GetComponent<GridTile>();

                //Create logic grid
                Vector3 tmp = new Vector3(i, 0, j);
                GridNode node = new GridNode(i, 0, j, tmp, gridTile);
                grid[i, j] = node;
            }
        }
    }


    public GridNode GetNode(int x, int z)
    {
        if (x >= 0 && x < currentConfig.SizeX && z >= 0 && z < currentConfig.SizeZ)
            return grid[x, z];
        return null;
    }

    public GridNode GetNodeFromWorldPosition(Vector3 pos)
    {
        foreach (GridNode n in grid)
        {
            if (n.WorldPosition.x == pos.x && n.WorldPosition.z == pos.z)
                return n;
        }

        return null;
    }

    private void CreateMapElements()
    {
        GenerateEntryPoint();
        GenerateExitPoint();
        GenerateEnemies();
        GenerateItems();
    }

    private void GenerateEntryPoint()
    {
        Vector3 pos = elementsConfig.DefaultEntryPos ? Vector3.zero : elementsConfig.CustomEntryPos;
        EntryPoint = GetNodeFromWorldPosition(pos);
        EntryPoint.IsEmpty = false;
        Element element = Instantiate(elementsConfig.EntryPrefrab, pos, Quaternion.identity, null).GetComponent<Element>();
        gameManager.RegisterTileElementPair(EntryPoint.Tile, element);
    }

    private void GenerateExitPoint()
    {
        Vector3 pos = elementsConfig.RandomEnemyPos ? GenerateRandomTile() : elementsConfig.CustomEntryPos;
        ExitPoint = GetNodeFromWorldPosition(pos);
        ExitPoint.IsEmpty = false;
        Element element = Instantiate(elementsConfig.ExitPrefab, pos, Quaternion.identity, null).GetComponent<Element>();
        gameManager.RegisterTileElementPair(ExitPoint.Tile, element);
    }

    private void GenerateEnemies()
    {
        foreach(EnemyConfig e in elementsConfig.enemies)
        {
            if (AreTilesAvailable())
            {
                Vector3 pos = elementsConfig.RandomEnemyPos ? GenerateRandomTile() : e.CustomEnemyPos;

                GridNode node = GetNodeFromWorldPosition(pos);

                if (!node.HasEnemy && node.IsEmpty)
                {
                    node.IsEmpty = false;
                    Enemy enemy = Instantiate(e.PrefabEnemy, pos, Quaternion.identity, null).GetComponent<Enemy>();
                    enemy.Node = node;
                    gameManager.RegisterTileElementPair(node.Tile, (Element)enemy);
                }
            }
         
        }
    }

    private void GenerateItems()
    {
        foreach (ItemConfig i in elementsConfig.items)
        {
            if (AreTilesAvailable())
            {
                Vector3 pos = elementsConfig.RandomEnemyPos ? GenerateRandomTile() : i.CustomItemPos;

                GridNode node = GetNodeFromWorldPosition(pos);

                if (!node.HasEnemy && node.IsEmpty)
                {
                    node.IsEmpty = false;
                    Item item = Instantiate(i.PrefabItem, pos, Quaternion.identity, null).GetComponent<Item>();
                    gameManager.RegisterTileElementPair(node.Tile, (Element)item);
                }
            }
        }
    }

    private Vector3 GenerateRandomTile()
    {
        Vector3 random = new Vector3(Random.Range(0, currentConfig.SizeX), Random.Range(0, currentConfig.SizeY), Random.Range(0, currentConfig.SizeZ));

        if (!GetNodeFromWorldPosition(random).HasEnemy || GetNodeFromWorldPosition(random).IsEmpty)
            return random;
        else
            return GenerateRandomTile();
    }

    private bool AreTilesAvailable()
    {
        foreach(GridNode node in grid)
        {
            if (!node.HasEnemy)
                return true;
        }

        return false;
    }

    public void UpdateTileNodes(GridTile tile)
    {
        UpdateExploredTiles(tile);
        UpdateExplorableNodes(tile);
    }

    private void UpdateExploredTiles(GridTile tile)
    {
        if (!tile.Node.IsExplored)
        {
            tile.Node.IsExplored = true;
            tile.SetMaterial(ExploredMat);
        }
    }

    private void UpdateExplorableNodes(GridTile tile)
    {
        playerTile = tile;

        List<GridNode> neightbours = Pathfinder.Instance.GetNeightbours(tile.Node, true);
        foreach(GridNode n in neightbours)
        {
            if (!n.IsExplored)
            {
                var dicc = gameManager.GetTilesElementsPairs();
                if (dicc.ContainsKey(n.Tile))
                {
                    dicc[n.Tile].gameObject.SetActive(true);

                    //Check if it is an enemy
                    Enemy enemy = null;
                    enemy = dicc[n.Tile] as Enemy;
                    if(enemy != null)
                    {
                        BlockNeightbourTiles(n.Tile, enemy);
                    }
                }

                n.IsExplorable = true;
                n.Tile.SetMaterial(ExplorableMat);
                explorableTiles.Add(n.Tile);
            }       
        }
    }

    private void BlockNeightbourTiles(GridTile tile, Enemy enemy)
    {
        List<GridNode> neightbours = Pathfinder.Instance.GetNeightbours(tile.Node, true);
        foreach (GridNode n in neightbours)
        {
            if(n.Tile != playerTile)
            {
                n.Tile.BlockTile();
                blockedTiles.Add(n.Tile);
                enemy.blockedTiles.Add(n.Tile);
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            foreach (GridNode pos in grid)
            {
                if (!pos.HasEnemy)
                    Gizmos.color = Color.green;
                else
                    Gizmos.color = Color.red;

                Gizmos.DrawWireCube(pos.WorldPosition, DebugCubeSize);
            }
        }       
    }

}
